using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventoryManagementSystem.Models
{
    public class ProductStock
    {
        public int ProductStockId { get; set; }
        public int ProductVendorId { get; set; }
        public int Quantity { get; set; }
        public DateTime StockTakeDate { get; set; }

        public virtual ProductVendor ProductVendor { get; set; }
    }
}