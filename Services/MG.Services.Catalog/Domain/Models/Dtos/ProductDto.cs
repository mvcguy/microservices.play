using System;
using System.ComponentModel.DataAnnotations;

namespace MG.Services.Catalog.Domain.Models.Dtos
{
    public class ProductDto
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(255)]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        [MaxLength(2048)]
        [MinLength(2)]
        public string Description { get; set; }

        [Required]
        [Range(0, 1000000)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, 1000000)]
        public decimal Quantity { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(3)]
        public string Currency { get; set; }

        public string CategoryId { get; set; }

    }
}
