using MG.Services.Catalog.Domain.Exceptions;
using MG.Services.Catalog.Domain.Models.Dtos;
using MG.Services.Catalog.Domain.Models.Mapping;
using MG.Services.Catalog.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MG.Services.Catalog.Domain
{
    public class ProductsDomain
    {
        private readonly IProductsRepository repository;
        private readonly PaginationPolicy paginationPolicy;

        public ProductsDomain(IProductsRepository repository,
            PaginationPolicy paginationPolicy)
        {
            this.repository = repository;
            this.paginationPolicy = paginationPolicy;
        }

        public IEnumerable<ProductDto> GetProductsByCategoryId(string categoryId, 
            int pageNumber)
        {
            int pageSize = GetPageSize();

            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }
            return repository.GetProductsByCategory(categoryId, pageNumber, pageSize).Select(x => x.ToDto());
        }

        public IEnumerable<ProductDto> GetProducts(int pageNumber)
        {
            int pageSize = GetPageSize();

            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }
            return repository.GetProducts(pageNumber, pageSize).Select(x => x.ToDto());
        }

        private int GetPageSize()
        {
            return this.paginationPolicy.MaxPageSize <= 0
                ? 100
                : this.paginationPolicy.MaxPageSize;
        }

        public ProductDto GetProduct(string id)
        {
            return repository.GetProduct(id).ToDto();
        }

        public ProductDto DeleteProduct(string id)
        {
            //
            // Soft delete only
            //
            var entry = repository.GetProduct(id);
            if (entry == null) throw new ItemNotFoundException("Product cannot be found");

            entry.Deleted = true;
            entry.ModifiedOn = DateTime.Now;

            repository.UpdateProduct(entry);

            return entry.ToDto();
        }

        public string AddProduct(ProductDto dto)
        {
            var model = dto.ToDbModel();

            model.Id = Guid.NewGuid().ToString();
            model.CreatedOn = DateTime.Now;
            model.ModifiedOn = DateTime.Now;

            repository.AddProduct(model);

            return model.Id;
        }

        public void UpdateProduct(ProductDto dto)
        {
            var entry = repository.GetProduct(dto.Id);
            if (entry == null) throw new ItemNotFoundException("Product cannot be found");

            entry.Name = dto.Name;
            entry.Description = dto.Description;
            entry.ModifiedOn = DateTime.Now;
            entry.Price = dto.Price;
            entry.Quantity = dto.Quantity;
            entry.Currency = dto.Currency;


            repository.UpdateProduct(entry);
        }

        public MetaDataDto GetProductsMetaData()
        {
            var count = repository.CountProducts();
            var pageSize = GetPageSize();
            var pages = count / (decimal)pageSize;

            return new MetaDataDto
            {
                TotalPages = (int)Math.Ceiling(pages),
                MaxPageSize = pageSize
            };
        }

        public int SaveChanges()
        {
            return repository.SaveChanges();
        }
    }
}
