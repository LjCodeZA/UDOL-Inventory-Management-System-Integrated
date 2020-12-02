using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventoryManagementSystem.Models
{
    public class ProductVendor
    {
        public int ProductVendorId { get; set; }
        public int ProductId { get; set; }
        public int VendorId { get; set; }
        public float Price { get; set; }
        public float ListPrice { get; set; }
        public virtual Product Product { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual ICollection<ProductStock> ProductStock { get; set; }
    }
}