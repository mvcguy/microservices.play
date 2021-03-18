using MG.Services.Catalog.Domain.Models.Db;
using MG.Services.Catalog.Domain.Models.Dtos;

namespace MG.Services.Catalog.Domain.Models.Mapping
{
    public static class ModelExtentions
    {
        public static ProductCategoryDto ToDto(this ProductCategory model)
        {
            if (model == null) return null;
            return new ProductCategoryDto
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description
            };
        }

        public static ProductCategory ToDbModel(this ProductCategoryDto model)
        {
            if (model == null) return null;
            return new ProductCategory
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description
            };
        }

        public static ProductDto ToDto(this Product model)
        {
            if (model == null) return null;
            return new ProductDto
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                CategoryId = model.CategoryId,
                Quantity = model.Quantity,
                Currency = model.Currency

            };
        }

        public static Product ToDbModel(this ProductDto model)
        {
            if (model == null) return null;
            return new Product
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                CategoryId = model.CategoryId,
                Quantity = model.Quantity,
                Currency = model.Currency
            };
        }
    }
}
