using System.ComponentModel.DataAnnotations;

namespace DockerWithNgnix.Models
{
    public class Product
    {
        [Key]
        public string? Id { get; set; }
        public string? Name { get; set; }

        [MaxLength(50)]
        public string? Description { get; set; }
        public string? Category { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

    }
}
