using CustomerProduct.Services;
using System.Text.Json;
using CustomerProduct.Data;
using CustomerProduct.DTOs;
using CustomerProduct.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace CustomerProduct.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly RabbitMQService _rabbitMQService;

        public ProductController(AppDbContext context, RabbitMQService rabbitMQService)
        {
            _context = context;
            _rabbitMQService = rabbitMQService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts([FromQuery] int? customerId)
        {
            var query = _context.Products.AsQueryable();

            if (customerId.HasValue)
            {
                query = query.Where(p => p.CustomerId == customerId.Value);
            }

            var products = await query
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    CustomerId = p.CustomerId
                })
                .ToListAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var dto = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CustomerId = product.CustomerId
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> PostProduct(ProductDTO productDTO)
        {
            var customerExists = await _context.Customers.AnyAsync(c => c.Id == productDTO.CustomerId);
            if (!customerExists)
            {
                return BadRequest($"CustomerId {productDTO.CustomerId} bulunamadı.");
            }

            var newProduct = new Product
            {
                Name = productDTO.Name,
                Price = productDTO.Price,
                CustomerId = productDTO.CustomerId
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            var message = JsonSerializer.Serialize(newProduct);
            _rabbitMQService.SendMessage("product_add_queue", message);

            productDTO.Id = newProduct.Id;
            return CreatedAtAction(nameof(GetProduct), new { id = newProduct.Id }, productDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductDTO productDTO)
        {
            if (id != productDTO.Id)
            {
                return BadRequest();
            }

            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = productDTO.Name;
            existingProduct.Price = productDTO.Price;
            existingProduct.CustomerId = productDTO.CustomerId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            _context.Products.Remove(existingProduct);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}