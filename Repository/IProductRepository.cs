using Products.model;

namespace Products.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProducts();

        Task<Product?> GetProductById(int pid);

        Task<Product> AddProduct(Product product);

        Task<bool> UpdateProduct(Product product);

        Task<bool> DeleteProduct(int pid);
    }
}