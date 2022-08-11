using EFCoreWithDapper.Database;
using EFCoreWithDapper.Database.Dapper;
using EFCoreWithDapper.Domain.Entities;
using EFCoreWithDapper.Models.Request;
using EFCoreWithDapper.Models.Response;
using EFCoreWithDapper.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EFCoreWithDapper.Services
{
    public class ProductService : IProductService
    {
        private readonly ApiDbContext _context;

        public ProductService(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductModel>> GetAllEFAsync(int? skip, int? take, CancellationToken ct)
        {
            return await (
                from p in _context.Set<ProductEntity>()
                select new
                {
                    Id = p.ExternalId,
                    p.Code,
                    p.Name,
                    MostRecentPriceHistory = p
                        .PricesHistory
                        .OrderByDescending(ph => ph.CreatedOn)
                        .First()
                })
                .OrderBy(p => p.Code)
                .Skip(skip ?? 0)
                .Take(take ?? 20)
                .Select(p => new ProductModel
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name,
                    Price = p.MostRecentPriceHistory.Price,
                    PriceChangedOn = p.MostRecentPriceHistory.CreatedOn
                })
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<ProductModel>> GetAllDapperAsync(int? skip, int? take, CancellationToken ct)
        {
            return await _context.QueryAsync<ProductModel>(ct, @"
                    SELECT p.ExternalId as IdString, p.Code, p.Name, lph.Price, lph.CreatedOn as PriceChangedOn
                    FROM (
                        SELECT Id, ExternalId, Code, Name, RowId
                        FROM Product
                        ORDER BY Code DESC
                        LIMIT @Take OFFSET @Skip
                    ) p
                    INNER JOIN (
                        SELECT ph.ProductId, ph.Price, ph.CreatedOn
                        FROM PriceHistory ph
                        INNER JOIN (
                            SELECT MAX(RowId) RowId
                            FROM PriceHistory
                            GROUP BY ProductId
                        ) phLatest ON ph.RowId = phLatest.RowId
                    ) lph ON p.Id = lph.ProductId", 
                    new
                    {
                        Skip = skip ?? 0,
                        Take = take ?? 20
                    });
        }

        public async Task<CreateProductResultModel> CreateEFAsync(CreateProductModel model, CancellationToken ct)
        {
            var externalId = Guid.NewGuid();

            await using var tx = await _context.Database.BeginTransactionAsync(ct);

            var product = new ProductEntity
            {
                ExternalId = externalId,
                Code = model.Code,
                Name = model.Name,
                PricesHistory =
                {
                    new PriceHistoryEntity
                    {
                        Price = model.Price,
                        CreatedOn = DateTime.UtcNow
                    }
                }
            };

            await _context.Set<ProductEntity>().AddAsync(product, ct);

            await _context.SaveChangesAsync(ct);

            await tx.CommitAsync(ct);

            return new CreateProductResultModel
            {
                Id = externalId
            };
        }

        public async Task<CreateProductResultModel> CreateDapperAsync(CreateProductModel model, CancellationToken ct)
        {
            var externalId = Guid.NewGuid();

            await using var tx = await _context.Database.BeginTransactionAsync(ct);

            await _context.ExecuteAsync(ct, @"
                INSERT INTO Product (ExternalId, Code, Name)
                VALUES (@ExternalId, @Code, @Name);
                INSERT INTO PriceHistory (Price, CreatedOn, ProductId)
                SELECT @Price, @CreatedOn, Id
                FROM Product
                WHERE
                    rowid = last_insert_rowid();", 
                    new
                {
                    ExternalId = externalId,
                    model.Code,
                    model.Name,
                    model.Price,
                    CreatedOn = DateTime.UtcNow
                });

            await tx.CommitAsync(ct);

            return new CreateProductResultModel
            {
                Id = externalId
            };
        }

        public async Task TruncateDapperAsync(CancellationToken ct)
        {
            var externalId = Guid.NewGuid();

            await using var tx = await _context.Database.BeginTransactionAsync(ct);

            await _context.ExecuteAsync(ct, @"
            DELETE FROM Product;");
            
            await tx.CommitAsync(ct);
        }
    }
}
