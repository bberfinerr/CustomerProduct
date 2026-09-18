using System.ComponentModel.DataAnnotations;

namespace CustomerProduct.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public int CustomerId { get; set; }
    }
}