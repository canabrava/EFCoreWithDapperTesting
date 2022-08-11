using EFCoreWithDapper.Domain.Entities;
using EFCoreWithDapper.Models.Request;
using EFCoreWithDapper.Models.Response;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace EFCoreWithDapper.IntegrationTest
{
    public class ProductControllerTests : IntegrationTest
    {
        [Fact]
        public async Task GetAllEFAsync_WithoutAnyProduct_ReturnsEmptyResponse()
        {
            // Arrange
            await TruncateProducts();

            // Act
            var response = await TestClient.GetAsync("products/EF");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<List<ProductModel>>()).Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllDapperAsync_WithoutAnyProduct_ReturnsEmptyResponse()
        {
            // Arrange
            await TruncateProducts();

            // Act
            var response = await TestClient.GetAsync("products/Dapper");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<List<ProductModel>>()).Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllDapperAsync_WithoutOneProduct_ReturnsEmptyResponse()
        {
            // Arrange
            var productModel = new CreateProductModel
            {
                Code = "1",
                Name = "ProductTest",
                Price = 10,
                PriceChangedOn = DateTime.Now
            };

            var createProductModel = await CreateProduct(productModel);

            // Act
            var response = await TestClient.GetAsync("products/Dapper");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnProducts = (await response.Content.ReadAsAsync<List<ProductModel>>());

            returnProducts.Should().HaveCount(1);
            returnProducts.Single().Id.Should().Equals(createProductModel.Id);
        }
    }
}
