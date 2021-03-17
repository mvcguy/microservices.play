using MG.WebSite.WebClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MG.WebSite.Models
{
    public class ProductCategoriesVm
    {
        public IEnumerable<ProductCategoryDto> ProductCategories { get; set; }

        public int PageNumber { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public ProductCategoriesVm()
        {
            ProductCategories = new List<ProductCategoryDto>();
        }
    }
}
