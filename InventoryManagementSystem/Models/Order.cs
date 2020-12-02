using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagementSystem.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int ProductVendorId { get; set; }
        public int Quantity { get; set; }
        public float TotalPrice { get; set; }
        public Nullable<bool> Recon { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}