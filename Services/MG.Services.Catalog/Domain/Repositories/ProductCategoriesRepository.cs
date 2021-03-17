using MG.Services.Catalog.Domain.Models.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MG.Services.Catalog.Domain.Repositories
{
    public class ProductCategoriesRepository : IProductCategoriesRepository
    {
        private readonly DbContext dbContext;
        private readonly DbSet<ProductCategory> productCategories;

        public ProductCategoriesRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.productCategories = dbContext.Set<ProductCategory>();
        }

        public void AddProductCategory(ProductCategory productCategory)
        {
            productCategories.Add(productCategory);
        }

        public IEnumerable<ProductCategory> GetProductCategories(int pageNumber, int pageSize)
        {
            var skip = (pageNumber - 1) * pageSize;
            var take = pageSize;



            return this.productCategories
                .Where(x => !x.Deleted)
                .OrderBy(x => x.Name)
                .Skip(skip)
                .Take(take);
        }

        public ProductCategory GetProductCategory(Guid id)
        {
            return productCategories.FirstOrDefault(x => !x.Deleted && x.Id == id);
        }

        public int SaveChanges()
        {
            return this.dbContext.SaveChanges();
        }

        public void UpdateProductCategory(ProductCategory productCategory)
        {
            productCategories.Update(productCategory);
        }
    }

}
