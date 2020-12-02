using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagementSystem.ViewModels
{
    public class ProductVendorStockGrouping
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public List<ProductVendorStock> ProductVendorStock { get; set; }
    }
}