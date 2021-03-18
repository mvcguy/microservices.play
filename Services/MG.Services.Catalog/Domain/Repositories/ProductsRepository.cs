using MG.Services.Catalog.Domain.Models.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MG.Services.Catalog.Domain.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly DbContext dbContext;
        private readonly DbSet<Product> products;

        public ProductsRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.products = dbContext.Set<Product>();
        }

        public void AddProduct(Product productCategory)
        {
            products.Add(productCategory);
        }

        public int CountProducts()
        {
            return this.products.Count();
        }

        public IEnumerable<Product> GetProductsByCategory(Guid categoryId,
            int pageNumber, int pageSize)
        {
            var skip = (pageNumber - 1) * pageSize;
            var take = pageSize;

            return this.products
                .Where(x => !x.Deleted && x.CategoryId == categoryId)
                .OrderBy(x => x.Name)
                .Skip(skip)
                .Take(take);
        }

        public IEnumerable<Product> GetProducts(int pageNumber, int pageSize)
        {
            var skip = (pageNumber - 1) * pageSize;
            var take = pageSize;

            return this.products
                .Where(x => !x.Deleted)
                .OrderBy(x => x.Name)
                .Skip(skip)
                .Take(take);
        }

        public Product GetProduct(Guid id)
        {
            return products.FirstOrDefault(x => !x.Deleted && x.Id == id);
        }

        public int SaveChanges()
        {
            return this.dbContext.SaveChanges();
        }

        public void UpdateProduct(Product productCategory)
        {
            products.Update(productCategory);
        }
    }


}
