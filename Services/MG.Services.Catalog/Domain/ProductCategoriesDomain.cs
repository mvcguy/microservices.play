using MG.Services.Catalog.Domain.Exceptions;
using MG.Services.Catalog.Domain.Models.Dtos;
using MG.Services.Catalog.Domain.Models.Mapping;
using MG.Services.Catalog.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MG.Services.Catalog.Domain
{
    public class ProductCategoriesDomain
    {
        private readonly IProductCategoriesRepository repository;
        private readonly PaginationPolicy paginationPolicy;

        public ProductCategoriesDomain(IProductCategoriesRepository repository, 
            PaginationPolicy paginationPolicy)
        {
            this.repository = repository;
            this.paginationPolicy = paginationPolicy;
        }

        public IEnumerable<ProductCategoryDto> GetProductCategories(int pageNumber)
        {
            int pageSize = GetPageSize();

            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }
            return repository.GetProductCategories(pageNumber, pageSize).Select(x => x.ToDto());
        }

        private int GetPageSize()
        {
            return this.paginationPolicy.MaxPageSize <= 0
                ? 100
                : this.paginationPolicy.MaxPageSize;
        }

        public ProductCategoryDto GetProductCategory(string id)
        {
            return repository.GetProductCategory(id).ToDto();
        }

        public ProductCategoryDto DeleteProductCategory(string id)
        {
            //
            // Soft delete only
            //
            var entry = repository.GetProductCategory(id);
            if (entry == null) throw new ItemNotFoundException("Product Category cannot be found");

            entry.Deleted = true;
            entry.ModifiedOn = DateTime.Now;

            repository.UpdateProductCategory(entry);

            return entry.ToDto();
        }

        public string AddProductCategory(ProductCategoryDto dto)
        {
            var model = dto.ToDbModel();

            model.Id = Guid.NewGuid().ToString();
            model.CreatedOn = DateTime.Now;
            model.ModifiedOn = DateTime.Now;

            repository.AddProductCategory(model);

            return model.Id;
        }

        public void UpdateProductCategory(ProductCategoryDto dto)
        {
            var entry = repository.GetProductCategory(dto.Id);
            if (entry == null) throw new ItemNotFoundException("Product Category cannot be found");

            entry.Name = dto.Name;
            entry.Description = dto.Description;
            entry.ModifiedOn = DateTime.Now;

            repository.UpdateProductCategory(entry);
        }

        public MetaDataDto GetProductCategoriesMetaData()
        {
            var count = repository.CountProductCategories();
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
