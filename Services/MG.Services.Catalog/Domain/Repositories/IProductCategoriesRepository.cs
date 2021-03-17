using MG.Services.Catalog.Domain.Models.Db;
using System;
using System.Collections.Generic;

namespace MG.Services.Catalog.Domain.Repositories
{
    public interface IProductCategoriesRepository
    {
        ProductCategory GetProductCategory(Guid id);

        IEnumerable<ProductCategory> GetProductCategories(int pageNumber, int pageSize);

        void AddProductCategory(ProductCategory productCategory);

        void UpdateProductCategory(ProductCategory productCategory);

        int SaveChanges();
    }
}
