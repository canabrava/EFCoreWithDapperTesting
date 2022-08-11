using Dapper;
using EFCoreWithDapper.Database;
using EFCoreWithDapper.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCoreWithDapper.Installers
{
    public static class SqlliteInstaller
    {
        private const string ConnectionString = "Data Source=EntityFrameworkCoreWithDapper;Mode=Memory;Cache=Shared";
        private static SqliteConnection _keepAliveConnection;

        public static void InstallSqlite(this IApplicationBuilder app)
        {
            _keepAliveConnection = new SqliteConnection(ConnectionString);
            _keepAliveConnection.Open();

            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
                ctx.Database.EnsureCreated();
            }
        }

        public static void AddSqlite(this IServiceCollection services)
        {
            SqlMapper.AddTypeHandler(new GuidTypeHandler());

            services.AddDbContext<ApiDbContext>(o =>
            {
                o.UseSqlite(ConnectionString);
            });
        }
    }
}
