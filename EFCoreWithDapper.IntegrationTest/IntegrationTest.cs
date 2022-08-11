using EFCoreWithDapper.Database;
using EFCoreWithDapper.Models.Request;
using EFCoreWithDapper.Models.Response;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http;
using System.Threading.Tasks;

namespace EFCoreWithDapper.IntegrationTest
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;
        public IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(ApiDbContext));
                        services.AddDbContext<ApiDbContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDb");
                        });
                    });
                });

            TestClient = appFactory.CreateClient();
        }

        protected async Task<CreateProductResultModel> CreateProduct(CreateProductModel model)
        {
            var response = await TestClient.PostAsJsonAsync("products/EF", model);

            return await response.Content.ReadAsAsync<CreateProductResultModel>();
        }

        protected async Task TruncateProducts()
        {
            var response = await TestClient.DeleteAsync("products/Dapper/Truncate");
        }
    }
}
