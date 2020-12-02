using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagementSystem.Models
{
    public class StockOut
    {
        public int StockOutId { get; set; }
        public int ProductVendorId { get; set; }
        public int Quantity { get; set; }
        public Nullable<bool> Recon { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ProductVendor ProductVendor { get; set; }
    }
}