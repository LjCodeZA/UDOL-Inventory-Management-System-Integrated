using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagementSystem.API.ViewModels
{
    public class ProductDashboardViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float ListPrice { get; set; }
        public string ImageURL { get; set; }
    }
}