using MG.Services.Catalog.Domain.Models.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MG.Services.Catalog.Domain.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ApplicationDbContext dbContext;
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

        public IEnumerable<Product> GetProductsByCategory(string categoryId,
            int pageNumber, int pageSize)
        {
            var skip = (pageNumber - 1) * pageSize;
            var take = pageSize;

            var list = this.products
                .OrderBy(x => x.Name)                
                .Where(x => x.Deleted == false && x.CategoryId == categoryId)
                .Skip(skip)
                .Take(take).ToList();
            return list;
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

        public Product GetProduct(string id)
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
