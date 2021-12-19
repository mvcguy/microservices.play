using MG.Services.Catalog.Domain.Models.Db;
using System;
using System.Collections.Generic;

namespace MG.Services.Catalog.Domain.Repositories
{
    public interface IProductCategoriesRepository
    {
        ProductCategory GetProductCategory(string id);

        IEnumerable<ProductCategory> GetProductCategories(int pageNumber, int pageSize);

        void AddProductCategory(ProductCategory productCategory);

        void UpdateProductCategory(ProductCategory productCategory);

        int CountProductCategories();

        int SaveChanges();
    }
}
