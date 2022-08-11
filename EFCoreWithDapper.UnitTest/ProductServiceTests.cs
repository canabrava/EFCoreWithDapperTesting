using System.Threading.Tasks;
using Xunit;
using EFCoreWithDapper.Services;
using EFCoreWithDapper.Database;
using System;
using EFCoreWithDapper.Domain.Entities;
using System.Threading;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EFCoreWithDapper.UnitTest
{
    public class ProductServiceTests : InMemoryDbTest
    {
        private readonly CancellationTokenSource _cancelationTokenSource;

        public ProductServiceTests() : 
            base(new DbContextOptionsBuilder<ApiDbContext>()
                .UseSqlite("Filename=Test.db")
                .Options)
        {
            _cancelationTokenSource = new CancellationTokenSource();
        }

        [Fact]
        public async Task GetAllEFAsync_WithProducts_ReturnsProducts()
        {
            using(var dbContext = new ApiDbContext(_dbContextOptions))
            {
                // Arrange
                var inTestService = new ProductService(dbContext);
                var token = _cancelationTokenSource.Token;

                var externalId = Guid.NewGuid();

                var product = new ProductEntity
                {
                    ExternalId = externalId,
                    Code = "1",
                    Name = "TestProject",
                    PricesHistory = {
                        new PriceHistoryEntity
                        {
                            Price = 5,
                            CreatedOn = DateTime.UtcNow
                        }
                    }
                };

                AddProduct(product);

                // Act
                var response = await inTestService.GetAllEFAsync(null, null, token);

                // Assert
                response.Should().HaveCount(1);
            }
            
        }

        [Fact]
        public async Task GetAllDapperAsync_WithProducts_ReturnsProducts()
        {
            using (var dbContext = new ApiDbContext(_dbContextOptions))
            {
                // Arrange
                var inTestService = new ProductService(dbContext);
                var token = _cancelationTokenSource.Token;

                var externalId = Guid.NewGuid();

                var product = new ProductEntity
                {
                    ExternalId = externalId,
                    Code = "1",
                    Name = "TestProject",
                    PricesHistory = {
                        new PriceHistoryEntity
                        {
                            Price = 5,
                            CreatedOn = DateTime.UtcNow
                        }
                    }
                };

                AddProduct(product);

                // Act
                var response = await inTestService.GetAllDapperAsync(null, null, token);

                // Assert
                response.Should().HaveCount(1);
            }

        }

    }
}
