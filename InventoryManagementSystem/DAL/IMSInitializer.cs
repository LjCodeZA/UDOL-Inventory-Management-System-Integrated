using InventoryManagementSystem.Models;
using System;
using System.Collections.Generic;

namespace InventoryManagementSystem.DAL
{
    public class IMSInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<IMSContext>
    {
        protected override void Seed(IMSContext context)
        {
            var products = new List<Product>
            {
                new Product{ ProductId=1,Name="Logitech MX518",Description="If you're going to play at the highest level, you need a mouse that gives you an edge. The Logitech MX 518 Gaming-Grade Optical Mouse...", ImageURL="../Images/logitech-mx518-2005.jpg"},
                new Product{ ProductId=2,Name="Razor DeathAdder Chroma",Description="The Razer DeathAdder Chroma is equipped with a 10,000dpi optical sensor, capable of mouse movement speeds of up to 300 inches per second...", ImageURL="../Images/Razer-deathadder-chroma_.jpg" },
                new Product{ ProductId=3,Name="Steelseries Rival 310",Description="Featuring 50-million click mechanical switches designed with Omron, the exclusive split-trigger buttons deliver long-lasting durability...", ImageURL="../Images/steelseries-rival-310.png" },
                new Product{ ProductId=4,Name="Corsair Void",Description="Hear everything in 7.1 surround sound from the lightest footstep to the most thundering explosion thanks to premium...", ImageURL="../Images/Corsair-void.jpg" },
                new Product{ ProductId=5,Name="Steelseries arctis 3",Description="Designed for everywhere you game, with superior sound, comfort and style on all gaming platforms, including PC, PlayStation...", ImageURL="../Images/steelseries-arctis-3.jpg" },
                new Product{ ProductId=6,Name="Kingston Hyper X Cloud 2",Description="Advanced USB audio control box with built-in DSP sound card, Hi-Fi capable with 53mm drivers for supreme audio quality...", ImageURL="../Images/hyperX.jpg" }
            };
            products.ForEach(s => context.Product.Add(s));
            context.SaveChanges();

            var vendor = new List<Vendor>
            {
                new Vendor { VendorId=1,Name="MouseCo",Description="Mouse provider" },
                new Vendor { VendorId=2,Name="Logitech",Description="Mouse provider" }
            };
            vendor.ForEach(s => context.Vendor.Add(s));
            context.SaveChanges();

            var productVender = new List<ProductVendor>
            {
                new ProductVendor{ ProductVendorId=1,ProductId=1,VendorId=1,Price=500f, ListPrice=600f },
                new ProductVendor{ ProductVendorId=2,ProductId=1,VendorId=2,Price=550f, ListPrice=600f },
                new ProductVendor{ ProductVendorId=3,ProductId=4,VendorId=1,Price=1200f, ListPrice=1400f },
                new ProductVendor{ ProductVendorId=4,ProductId=4,VendorId=2,Price=1300f, ListPrice=1400f },
                new ProductVendor{ ProductVendorId=5,ProductId=2,VendorId=1,Price=700f, ListPrice=1000f },
                new ProductVendor{ ProductVendorId=6,ProductId=3,VendorId=1,Price=300f, ListPrice=600f },
                new ProductVendor{ ProductVendorId=7,ProductId=5,VendorId=1,Price=1500f, ListPrice=1800f },
                new ProductVendor{ ProductVendorId=8,ProductId=6,VendorId=1,Price=800f, ListPrice=1000f },
            };
            productVender.ForEach(s => context.ProductVendor.Add(s));
            context.SaveChanges();

            var productStock = new List<ProductStock>
            {
                new ProductStock{ ProductStockId=1, ProductVendorId=1, Quantity=50, StockTakeDate=DateTime.Now},
                new ProductStock{ ProductStockId=2, ProductVendorId=2, Quantity=50, StockTakeDate=DateTime.Now},
                new ProductStock{ ProductStockId=3, ProductVendorId=3, Quantity=20, StockTakeDate=DateTime.Now},
                new ProductStock{ ProductStockId=4, ProductVendorId=4, Quantity=40, StockTakeDate=DateTime.Now},
                new ProductStock{ ProductStockId=5, ProductVendorId=4, Quantity=60, StockTakeDate=DateTime.Now.AddDays(-1)},
            };
            productStock.ForEach(s => context.ProductStock.Add(s));
            context.SaveChanges();

            var stockOut = new List<StockOut>
            {
                new StockOut { StockOutId = 1, ProductVendorId = 1, Quantity = 20, CreatedDate=DateTime.Now },
                new StockOut { StockOutId = 2, ProductVendorId = 3, Quantity = 10, CreatedDate=DateTime.Now }
            };
            stockOut.ForEach(s => context.StockOut.Add(s));
            context.SaveChanges();

        }
    }
}