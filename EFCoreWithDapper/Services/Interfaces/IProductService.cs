using EFCoreWithDapper.Models.Request;
using EFCoreWithDapper.Models.Response;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EFCoreWithDapper.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductModel>> GetAllEFAsync(int? skip, int? take, CancellationToken ct);
        Task<IEnumerable<ProductModel>> GetAllDapperAsync(int? skip, int? take, CancellationToken ct);
        Task<CreateProductResultModel> CreateEFAsync(CreateProductModel model, CancellationToken ct);
        Task<CreateProductResultModel> CreateDapperAsync(CreateProductModel model, CancellationToken ct);
        Task TruncateDapperAsync(CancellationToken ct);
    }
}
