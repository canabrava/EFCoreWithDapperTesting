using EFCoreWithDapper.Models.Request;
using EFCoreWithDapper.Models.Response;
using EFCoreWithDapper.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EFCoreWithDapper.Controllers
{
    [Route("products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("EF")]
        public async Task<IEnumerable<ProductModel>> GetAllEFAsync([FromQuery] int? skip, [FromQuery] int? take, CancellationToken ct)
        {
            return await _productService.GetAllEFAsync(skip, take, ct);
        }

        [HttpGet("Dapper")]
        public async Task<IEnumerable<ProductModel>> GetAllDapperAsync([FromQuery] int? skip, [FromQuery] int? take, CancellationToken ct)
        {
            return await _productService.GetAllDapperAsync(skip, take, ct);
        }

        [HttpPost("EF")]
        public async Task<CreateProductResultModel> CreateEFAsync([FromBody] CreateProductModel model, CancellationToken ct)
        {
            return await _productService.CreateEFAsync(model, ct);
        }

        [HttpPost("Dapper")]
        public async Task<CreateProductResultModel> CreateDapperAsync([FromBody] CreateProductModel model, CancellationToken ct)
        {
            return await _productService.CreateDapperAsync(model, ct);
        }

        [HttpDelete("Dapper/Truncate")]
        public async Task TruncateDapperAsync(CancellationToken ct)
        {
            await _productService.TruncateDapperAsync(ct);
        }
    }
}

