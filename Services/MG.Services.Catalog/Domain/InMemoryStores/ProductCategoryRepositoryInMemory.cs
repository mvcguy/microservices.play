using MG.Services.Catalog.Domain.Models.Db;
using MG.Services.Catalog.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MG.Services.Catalog.Domain.InMemoryStores
{
    public class ProductCategoryRepositoryInMemory : IProductCategoriesRepository
    {
        private readonly ProductCategoriesInMemory productCategories;
        private readonly PaginationPolicy paginationPolicy;

        public ProductCategoryRepositoryInMemory(ProductCategoriesInMemory productCategories,
            PaginationPolicy paginationPolicy)
        {
            this.productCategories = productCategories;
            this.paginationPolicy = paginationPolicy;
        }

        public void AddProductCategory(ProductCategory productCategory)
        {
            this.productCategories.Add(productCategory);
        }

        public int CountProductCategories()
        {
            return this.productCategories.Count();
        }

        public IEnumerable<ProductCategory> GetProductCategories(int pageNumber, int pageSize)
        {           
            var skip = (pageNumber - 1) * pageSize;
            var take = pageSize;

            return this.productCategories.Where(x=>!x.Deleted).OrderBy(x => x.CreatedOn).Skip(skip).Take(take);
        }

        public ProductCategory GetProductCategory(string id)
        {
            return productCategories.FirstOrDefault(x => x.Id == id && !x.Deleted);
        }

        public int SaveChanges()
        {
            return 1;
        }

        public void UpdateProductCategory(ProductCategory productCategory)
        {
            
        }
    }
}
