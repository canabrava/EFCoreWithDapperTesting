using EFCoreWithDapper.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EFCoreWithDapper.Services.DependencyInjection
{
    public static class ServicesDependecyInjection
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
        }
    }
}
