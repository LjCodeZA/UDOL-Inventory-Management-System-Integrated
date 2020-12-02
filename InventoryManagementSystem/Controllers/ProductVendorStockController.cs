using InventoryManagementSystem.DAL;
using InventoryManagementSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryManagementSystem.Controllers
{
    public class ProductVendorStockController : Controller
    {
        // GET: ProductVendorStock
        // https://www.tutlane.com/tutorial/aspnet-mvc/how-to-use-viewmodel-in-asp-net-mvc-with-example
        /*
         P.ProductId
		,P.Name
		,V.VendorId
		,V.Name
		,PS.ProductStockId
		,PS.ProductVendorId
		,PS.Quantity
		,PS.StockTakeDate
         */
        public ActionResult Index()
        {
            var dbContext = new IMSContext();
            var productVendorStockList = new List<ProductVendorStock>();

            var productVendorStock = (from productStock in dbContext.ProductStock
                                      join productVendor in dbContext.ProductVendor 
                                        on productStock.ProductVendorId equals productVendor.ProductVendorId
                                       join product in dbContext.Product
                                        on productVendor.ProductId equals product.ProductId
                                        join vendor in dbContext.Vendor
                                        on productVendor.VendorId equals vendor.VendorId 
                                      select new { product.ProductId,
                                                   ProductName = product.Name,
                                                   vendor.VendorId,
                                                   VendorName = vendor.Name,
                                                   productStock.ProductStockId,
                                                   productStock.ProductVendorId,
                                                   productStock.Quantity,
                                                   productStock.StockTakeDate}).ToList();

            foreach (var item in productVendorStock)
            {
                var productVendorStockItem = new ProductVendorStock();

                productVendorStockItem.ProductId = item.ProductId;
                productVendorStockItem.ProductName = item.ProductName;
                productVendorStockItem.VendorId = item.VendorId;
                productVendorStockItem.VendorName = item.VendorName;
                productVendorStockItem.ProductStockId = item.ProductStockId;
                productVendorStockItem.ProductVendorId = item.ProductVendorId;
                productVendorStockItem.Quantity = item.Quantity;
                productVendorStockItem.StockTakeDate = item.StockTakeDate;

                productVendorStockList.Add(productVendorStockItem);
            }

            return View(productVendorStockList);
        }
    }
}