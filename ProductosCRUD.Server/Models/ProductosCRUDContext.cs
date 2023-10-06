using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace ProductosCRUD.Server.Models
{
    public class ProductosCRUDContext : DbContext
    {
        public ProductosCRUDContext(DbContextOptions<ProductosCRUDContext> options) : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);           
            modelBuilder.Entity<Producto>().HasIndex(c => c.Nombre).IsUnique();           
        }
    }
}
