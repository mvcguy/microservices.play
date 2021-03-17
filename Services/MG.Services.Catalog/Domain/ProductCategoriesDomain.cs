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
            var pageSize =
                this.paginationPolicy.MaxPageSize <= 0
                ? 100
                : this.paginationPolicy.MaxPageSize;

            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }
            return repository.GetProductCategories(pageNumber, pageSize).Select(x => x.ToDto());
        }

        public ProductCategoryDto GetProductCategory(Guid id)
        {
            return repository.GetProductCategory(id).ToDto();
        }

        public ProductCategoryDto DeleteProductCategory(Guid id)
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

        public Guid AddProductCategory(ProductCategoryDto dto)
        {
            var model = dto.ToDbModel();

            model.Id = Guid.NewGuid();
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

        public int SaveChanges()
        {
            return repository.SaveChanges();
        }
    }

    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string message) : base(message)
        {
        }
    }
}
