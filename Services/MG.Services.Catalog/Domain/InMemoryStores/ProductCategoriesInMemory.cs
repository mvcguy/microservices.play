using MG.Services.Catalog.Domain.Models.Db;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MG.Services.Catalog.Domain.InMemoryStores
{
    public class ProductCategoriesInMemory : ICollection<ProductCategory>
    {
        private List<ProductCategory> _collection;

        public ProductCategoriesInMemory()
        {
            _collection = new List<ProductCategory>();
        }

        public int Count => _collection.Count;
        public bool IsReadOnly => false;

        public void Add(ProductCategory item)
        {
            _collection.Add(item);
        }

        public void Clear()
        {
            _collection.Clear();
        }

        public bool Contains(ProductCategory item)
        {
            return _collection.Contains(item);
        }

        public void CopyTo(ProductCategory[] array, int arrayIndex)
        {
            _collection.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ProductCategory> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        public bool Remove(ProductCategory item)
        {
            return _collection.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        public void Seed()
        {
            var items = Enumerable.Range(1, 500).Select(index => new ProductCategory
            {
                Id = Guid.NewGuid(),
                Name = $"00{index}-CAT",
                Description = $"_blank_{index}",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            });

            _collection.AddRange(items);
        }
    }
}
