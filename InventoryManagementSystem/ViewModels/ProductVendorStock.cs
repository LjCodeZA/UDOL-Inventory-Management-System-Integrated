using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagementSystem.ViewModels
{
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

    public class ProductVendorStock
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public int ProductStockId { get; set; }
        public int ProductVendorId { get; set; }
        public int Quantity { get; set; }
        public DateTime StockTakeDate { get; set; }
    }
}