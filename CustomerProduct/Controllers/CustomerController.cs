using CustomerProduct.Data;
using CustomerProduct.DTOs;
using CustomerProduct.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerProduct.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomers()
        {
            var customers = await _context.Customers
                .Select(c => new CustomerDTO
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email
                })
                .ToListAsync();

            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDTO>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var dto = new CustomerDTO
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDTO>> PostCustomer(CustomerDTO customerDTO)
        {
            var newCustomer = new Customer
            {
                FirstName = customerDTO.FirstName,
                LastName = customerDTO.LastName,
                Email = customerDTO.Email
            };

            _context.Customers.Add(newCustomer);
            await _context.SaveChangesAsync();

            customerDTO.Id = newCustomer.Id;
            return CreatedAtAction(nameof(GetCustomer), new { id = newCustomer.Id }, customerDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerDTO customerDTO)
        {
            if (id != customerDTO.Id)
            {
                return BadRequest();
            }

            var existingCustomer = await _context.Customers.FindAsync(id);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            existingCustomer.FirstName = customerDTO.FirstName;
            existingCustomer.LastName = customerDTO.LastName;
            existingCustomer.Email = customerDTO.Email;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var existingCustomer = await _context.Customers.FindAsync(id);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(existingCustomer);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}