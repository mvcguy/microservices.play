using MG.Services.Catalog.Domain.Models.Db;
using MG.Services.Catalog.Domain.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
