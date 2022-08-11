using EFCoreWithDapper.Database;
using EFCoreWithDapper.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCoreWithDapper.UnitTest
{
    public class InMemoryDbTest
    {
        protected DbContextOptions<ApiDbContext> _dbContextOptions;

        public InMemoryDbTest(DbContextOptions<ApiDbContext> contextOptions)
        {
            _dbContextOptions = contextOptions;

            CreateDb();
        }

        public void CreateDb()
        {
            using (var context = new ApiDbContext(_dbContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }

        public void AddProduct(ProductEntity product)
        {
            using (var context = new ApiDbContext(_dbContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Add(product);
                context.SaveChanges();
            }
        }
    }
}
