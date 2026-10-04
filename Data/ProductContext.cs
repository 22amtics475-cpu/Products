using Microsoft.EntityFrameworkCore;
using Products.dto;
using Products.model;


namespace Products.Data
{
    public class ProductContext(DbContextOptions<ProductContext> options):DbContext(options)
    {
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Otp> Otps { get; set; }
    }
}
