using System;

namespace MG.Services.Catalog.Domain.Models.Db
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal Quantity { get; set; }

        public string Currency { get; set; }

        public Guid CategoryId { get; set; }

        public string Description { get; set; }

        public bool Deleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
    }

}
