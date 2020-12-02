using InventoryManagementSystem.DAL;
using InventoryManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagementSystem.Common
{
    public class CommonUse
    {
        private IMSContext db = new IMSContext();
        public void DoStockAllocation(StockOut stockOut)
        {
            var dbContext = new IMSContext();

            var productId = (from productVendor in dbContext.ProductVendor where productVendor.ProductVendorId == stockOut.ProductVendorId select productVendor.ProductId).FirstOrDefault();

            var allVendorsForProduct = (from productVendors in dbContext.ProductVendor where productVendors.ProductId == productId select productVendors); //Exclude current selected vendor

            //Does the selected vendor have enough stock?
            var stockTakenOutButNotReconned = (from stockTakenOut in dbContext.StockOut where stockTakenOut.ProductVendorId == stockOut.ProductVendorId && (stockTakenOut.Recon == false || stockTakenOut.Recon == null) select (int?)stockTakenOut.Quantity).Sum() ?? 0;

            var latestStockTake = (from productStock in dbContext.ProductStock
                                   group productStock by productStock.ProductVendorId
                                   into latestStock
                                   select latestStock.OrderByDescending(t => t.StockTakeDate).FirstOrDefault());

            var amountOfStockReconnedForVendor = latestStockTake.Where(i => i.ProductVendorId == stockOut.ProductVendorId).Select(o => (int?)o.Quantity).Sum() ?? 0;

            if (amountOfStockReconnedForVendor - (stockTakenOutButNotReconned + stockOut.Quantity) < 0)
            {
                //Not enough stock for order from this vendor. Need to check different vendors for stock.
                //We start off by checking if we have enough in total.
                var stockTakenOutButNotReconnedForProductVendors = (from stockTakenOut in dbContext.StockOut where allVendorsForProduct.Select(i => i.ProductVendorId).Contains(stockTakenOut.ProductVendorId) && (stockTakenOut.Recon == false || stockTakenOut.Recon == null) select (int?)stockTakenOut.Quantity).Sum() ?? 0;
                var amountOfStockReconnedForProductVendors = latestStockTake.Where(i => allVendorsForProduct.Select(vendors => vendors.ProductVendorId).Contains(i.ProductVendorId)).Select(o => o.Quantity).DefaultIfEmpty().Sum();

                if (amountOfStockReconnedForProductVendors - (stockTakenOutButNotReconnedForProductVendors + stockOut.Quantity) >= 0)
                {
                    //We have enough total stock to service the request

                    var totalStockRequested = stockOut.Quantity;
                    var remainingStockToAllocate = totalStockRequested;

                    //Start by allocating stock from chosen vendor

                    var currentVendorStock = amountOfStockReconnedForVendor - stockTakenOutButNotReconned;


                    stockOut.Quantity = currentVendorStock;
                    db.StockOut.Add(stockOut);
                    db.SaveChanges();

                    remainingStockToAllocate -= currentVendorStock;

                    var alternativeProductVendors = (from productVendors in dbContext.ProductVendor where productVendors.ProductVendorId != stockOut.ProductVendorId && productVendors.ProductId == productId select productVendors); //Exclude current selected vendor
                    foreach (var item in alternativeProductVendors)
                    {
                        var amountOfStockReconnedForAlternativeVendor = latestStockTake.Where(i => i.ProductVendorId == item.ProductVendorId).Select(o => o.Quantity).Sum();
                        var stockTakenOutButNotReconnedForAlternativeVendor = (from stockTakenOut in dbContext.StockOut where stockTakenOut.ProductVendorId == item.ProductVendorId && (stockTakenOut.Recon == false || stockTakenOut.Recon == null) select (int?)stockTakenOut.Quantity).Sum() ?? 0;

                        currentVendorStock = amountOfStockReconnedForAlternativeVendor - stockTakenOutButNotReconnedForAlternativeVendor;
                        if (currentVendorStock == 0)
                            continue;

                        if (currentVendorStock >= remainingStockToAllocate)
                        {
                            db.StockOut.Add(new StockOut()
                            {
                                ProductVendorId = item.ProductVendorId,
                                CreatedDate = DateTime.Now,
                                Quantity = remainingStockToAllocate,
                                Recon = null
                            });

                            db.SaveChanges();

                            break;
                        }
                        else
                        {
                            db.StockOut.Add(new StockOut()
                            {
                                ProductVendorId = item.ProductVendorId,
                                CreatedDate = DateTime.Now,
                                Quantity = currentVendorStock,
                                Recon = null
                            });

                            db.SaveChanges();
                        }

                        remainingStockToAllocate -= currentVendorStock;
                    }
                }
                else
                {
                    //We don't have enough stock at all. Stop process.
                    return;
                }
            }
            else
            {
                //Enough stock from the selected vendor. Carry on with order.

                db.StockOut.Add(stockOut);
                db.SaveChanges();
                //return RedirectToAction("Index");
            }
        }
    }
}