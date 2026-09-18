using System.ComponentModel.DataAnnotations;

namespace CustomerProduct.DTOs
{
    public class CustomerDTO
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
    }
}