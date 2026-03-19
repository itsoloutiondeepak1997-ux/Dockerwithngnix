using DockerWithNgnix.DataLayer;
using DockerWithNgnix.DTO;
using DockerWithNgnix.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DockerWithNgnix.Controllers
{
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
                _context = context;
        }

        /// <summary>
        /// Get the list of all products
        /// </summary>
        /// <returns></returns>

        [HttpGet("ProductList")]
        public async Task<IActionResult> ProductList()
        {
            var productlist = _context.Products.ToList();
            return Ok(productlist);
        }
        /// <summary>
        /// This method is used to add a new product.
        /// </summary>
        /// <param name="productDto">Product data to be added.</param>
        /// <returns>Returns the created product with status code 201.</returns>

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = new Product()
            {
                Id = Guid.NewGuid().ToString(),
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Category = productDto.Category
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(new { statuscode = 200, message = "Product added successfully" });
        }


        [HttpPost("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = new Product()
            {
                Id = Guid.NewGuid().ToString(),
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Category = productDto.Category
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(new { statuscode = 200, message = "Product Update successfully" });
        }



    }
}
