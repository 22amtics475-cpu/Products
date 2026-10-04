using Microsoft.EntityFrameworkCore;
using Products.Data;
using Products.model;

namespace Products.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;

        public ProductRepository(ProductContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetProductById(int pid)
        {
            return await _context.Products.FindAsync(pid);
        }

        public async Task<Product> AddProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var existingProduct = await _context.Products.FindAsync(product.pid);

            if (existingProduct == null)
            {
                return false;
            }

            existingProduct.name = product.name;
            existingProduct.prize = product.prize;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProduct(int pid)
        {
            var product = await _context.Products.FindAsync(pid);

            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}