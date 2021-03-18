using MG.Services.Catalog.Domain.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace MG.Services.Catalog.Domain.Repositories
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<ProductCategory> ProductCategories { get; set; }

        public virtual DbSet<Product> Products { get; set; }
    }

}
