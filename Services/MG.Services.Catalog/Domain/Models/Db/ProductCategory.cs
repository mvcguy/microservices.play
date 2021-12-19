using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MG.Services.Catalog.Domain.Models.Db
{
    public class ProductCategory
    {
        [Key]        
        public string Id { get; set; }

        public string Name { get; set; }

        public bool Deleted { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
    }

}
