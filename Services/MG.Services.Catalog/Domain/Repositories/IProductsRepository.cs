using MG.Services.Catalog.Domain.Models.Db;
using System;
using System.Collections.Generic;

namespace MG.Services.Catalog.Domain.Repositories
{
    public interface IProductsRepository
    {
        void AddProduct(Product productCategory);
        int CountProducts();
        Product GetProduct(string id);
        IEnumerable<Product> GetProducts(int pageNumber, int pageSize);
        IEnumerable<Product> GetProductsByCategory(string categoryId, int pageNumber, int pageSize);
        int SaveChanges();
        void UpdateProduct(Product productCategory);
    }
}