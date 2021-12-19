using MG.WebSite.WebClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MG.WebSite.Models
{

    public class BaseVm
    {
        public int PageNumber { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }
    }

    public class ProductCategoriesVm : BaseVm
    {
        public IEnumerable<ProductCategoryDto> ProductCategories { get; set; }

        public ProductCategoriesVm()
        {
            ProductCategories = new List<ProductCategoryDto>();
        }
    }

    public class ProductVm : BaseVm
    {

        public Guid CategoryId { get; set; }

        public IEnumerable<ProductDto> Products { get; set; }

        public ProductVm()
        {
            Products = new List<ProductDto>();
        }
    }
}
