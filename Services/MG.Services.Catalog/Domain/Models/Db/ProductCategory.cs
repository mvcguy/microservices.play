using System;
using System.ComponentModel.DataAnnotations;

namespace MG.Services.Catalog.Domain.Models.Db
{
    public class ProductCategory
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool Deleted { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
    }

}
