using System;
using System.ComponentModel.DataAnnotations;

namespace MG.Services.Catalog.Domain.Models.Dtos
{
    public class ProductCategoryDto
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
    }
}
