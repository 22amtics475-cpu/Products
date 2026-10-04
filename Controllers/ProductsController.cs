using Microsoft.AspNetCore.Mvc;
using Products.ApiResponse;
using Products.dto;
using Products.model;
using Products.Repository;
using Microsoft.AspNetCore.Authorization;

namespace Products.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _repository.GetAllProducts();

            var response = new ApiResponse<List<Product>>
            {
                Success = true,
                Message = "Products retrieved successfully",
                Data = products
            };

            return Ok(response);
        }

        // GET: api/Products/5
        [HttpGet("{pid}")]
        public async Task<IActionResult> GetProductById(int pid)
        {
            var product = await _repository.GetProductById(pid);

            if (product == null)
            {
                return NotFound();
            }

            var response = new ApiResponse<Product>
            {
                Success = true,
                Message = "success",
                Data = product
            };

            return Ok(response);
        }

        // POST: api/Products
        [HttpPost]
        public async Task<IActionResult> AddProduct(CreateProducts product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newProduct = new Product
            {
                pid = product.pid,
                name = product.name,
                prize = product.prize
            };

            var result = await _repository.AddProduct(newProduct);

            var response = new ApiResponse<Product>
            {
                Success = true,
                Message = "Added",
                Data = result
            };

            return Ok(response);
        }

        // PUT: api/Products/5
        [HttpPut("{pid}")]
        public async Task<IActionResult> UpdateProduct(int pid, Product product)
        {
            if (pid != product.pid)
            {
                return BadRequest();
            }

            var updated = await _repository.UpdateProduct(product);

            if (!updated)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // DELETE: api/Products/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{pid}")]
        public async Task<IActionResult> DeleteProduct(int pid)
        {
            var deleted = await _repository.DeleteProduct(pid);

            if (!deleted)
            {
                return NotFound();
            }

            var response = new ApiResponse<bool>
            {
                Success = true,
                Message = "Product deleted successfully",
                Data = true
            };

            return Ok(response);
        }
    }
}