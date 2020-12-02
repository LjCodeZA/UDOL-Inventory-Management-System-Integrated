using InventoryManagementSystem.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace InventoryManagementSystem.DAL
{
    public class IMSContext : DbContext
    {
        public IMSContext() : base("IMSContext")
        {
        }

        public DbSet<Product> Product { get; set; }
        public DbSet<Vendor> Vendor { get; set; }
        public DbSet<ProductStock> ProductStock { get; set; }
        public DbSet<ProductVendor> ProductVendor { get; set; }
        public DbSet<StockOut> StockOut { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}