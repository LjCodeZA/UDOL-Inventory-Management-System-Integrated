using InventoryManagementSystem.Common;
using InventoryManagementSystem.DAL;
using InventoryManagementSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryManagementSystem.Controllers
{
    public class ProductVendorStockGroupingController : Controller
    {
        // GET: ProductVendorStockGrouping
        public ActionResult Index()
        {
            var rabbitMQConsumer = new RabbitMQConnector();
            rabbitMQConsumer.ConsumeMessages();

            var dbContext = new IMSContext();

            var productVendorStockGroupingList = new List<ProductVendorStockGrouping>();

            var productVendorStockTemp = (from productStock in dbContext.ProductStock
                                          join productVendor in dbContext.ProductVendor
                                            on productStock.ProductVendorId equals productVendor.ProductVendorId
                                          join product in dbContext.Product
                                           on productVendor.ProductId equals product.ProductId
                                          join vendor in dbContext.Vendor
                                          on productVendor.VendorId equals vendor.VendorId
                                          select new
                                          {
                                              product.ProductId,
                                              ProductName = product.Name,
                                              vendor.VendorId,
                                              VendorName = vendor.Name,
                                              productStock.ProductStockId,
                                              productStock.ProductVendorId,
                                              productStock.Quantity,
                                              productStock.StockTakeDate
                                          }).ToList();

            var productVendorStock = (from productStock in productVendorStockTemp
                                      group productStock by productStock.ProductVendorId
                                      into latestStock
                                      select latestStock.OrderByDescending(t => t.StockTakeDate).FirstOrDefault());


            foreach (var item in productVendorStock)
            {
                var productVendorStockGroupingItem = new ProductVendorStockGrouping();
                var productVendorStockItem = new ProductVendorStock();

                productVendorStockItem.ProductId = item.ProductId;
                productVendorStockItem.ProductName = item.ProductName;
                productVendorStockItem.VendorId = item.VendorId;
                productVendorStockItem.VendorName = item.VendorName;
                productVendorStockItem.ProductStockId = item.ProductStockId;
                productVendorStockItem.ProductVendorId = item.ProductVendorId;
                productVendorStockItem.Quantity = item.Quantity;
                productVendorStockItem.StockTakeDate = item.StockTakeDate;

                var existingProductStock = productVendorStockGroupingList.Where(i => i.ProductId == item.ProductId).SingleOrDefault();

                if (existingProductStock != null)
                {
                    existingProductStock.Quantity += item.Quantity;
                    existingProductStock.ProductVendorStock.Add(productVendorStockItem);

                }
                else
                {
                    productVendorStockGroupingItem.Quantity = item.Quantity;
                    productVendorStockGroupingItem.ProductId = item.ProductId;
                    productVendorStockGroupingItem.ProductVendorStock = new List<ProductVendorStock>();
                    productVendorStockGroupingItem.ProductVendorStock.Add(productVendorStockItem);
                    productVendorStockGroupingList.Add(productVendorStockGroupingItem);


                }
            }

            return View(productVendorStockGroupingList);
        }

        public int RecalculateStock()
        {
            var dbContext = new IMSContext();

            var NeedsRecon = (from stockOut in dbContext.StockOut where stockOut.Recon == false || stockOut.Recon == null select stockOut);

            if (NeedsRecon != null && NeedsRecon.Count() > 0)
            {
                var LatestStockTake = (from productStock in dbContext.ProductStock
                                       group productStock by productStock.ProductVendorId
                                       into latestStock
                                       select latestStock.OrderByDescending(t => t.StockTakeDate).FirstOrDefault());

                var itemsToRecon = LatestStockTake.Where(i => NeedsRecon.Any(c => c.ProductVendorId == i.ProductVendorId));
                var itemsToLeave = LatestStockTake.Where(i => NeedsRecon.All(c => c.ProductVendorId != i.ProductVendorId));

                var productStockUpdate = new List<Models.ProductStock>();

                foreach (var item in NeedsRecon)
                {
                    var newEntry = new Models.ProductStock();
                    var NeedsReconItem = NeedsRecon.Where(i => i.StockOutId == item.StockOutId).FirstOrDefault();
                    var itemStockQuantity = LatestStockTake.Where(i => i.ProductVendorId == item.ProductVendorId).FirstOrDefault().Quantity;

                    newEntry.StockTakeDate = DateTime.Now;
                    newEntry.ProductVendorId = item.ProductVendorId;

                    var entryExists = productStockUpdate.Where(i => i.ProductVendorId == item.ProductVendorId).FirstOrDefault();
                    if (entryExists != null)
                        entryExists.Quantity -= NeedsReconItem.Quantity;
                    else
                        newEntry.Quantity = itemStockQuantity - NeedsReconItem.Quantity;

                    if (entryExists != null)
                        entryExists.Quantity -= newEntry.Quantity;
                    else
                        productStockUpdate.Add(newEntry);

                    NeedsReconItem.Recon = true;
                }

                foreach (var newEntry in productStockUpdate)
                {
                    dbContext.ProductStock.Add(newEntry);
                }

                foreach (var item in itemsToLeave)
                {
                    var entryToKeep = item;
                    entryToKeep.StockTakeDate = DateTime.Now;

                    dbContext.ProductStock.Add(entryToKeep);
                }

                dbContext.SaveChanges();
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}