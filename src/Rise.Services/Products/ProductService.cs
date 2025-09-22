using Microsoft.EntityFrameworkCore;
using Rise.Domain.Products;
using Rise.Persistence;
using Rise.Shared.Common;
using Rise.Shared.Products;

namespace Rise.Services.Products;

public class ProductService(ApplicationDbContext dbContext) : IProductService
{
    public async Task<Result<ProductResponse.Create>> CreateAsync(ProductRequest.Create request, CancellationToken ctx)
    {
        if (await dbContext.Products.AnyAsync(x => x.Name == request.Name))
        {
            Log.Warning("Product with name '{Name}' already exists.", request.Name);
            return Result.Conflict($"Product with name '{request.Name}'  already exists.");
        }
        
        Product p = new(request.Name, request.Description);
        
        dbContext.Products.Add(p);
        
        await dbContext.SaveChangesAsync(ctx);
        
        return Result.Created(new ProductResponse.Create
        {
            ProductId = p.Id,
        });
    }

    public async Task<Result<ProductResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request, CancellationToken ctx)
    {
        var query = dbContext.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(p => p.Name.Contains(request.SearchTerm) || p.Description.Contains(request.SearchTerm));
        }
        
        var totalCount = await query.CountAsync(ctx);
        
        var products = await query.AsNoTracking()
            .OrderBy(p => p.Name)
            .Skip(request.Skip)
            .Take(request.Take)
            .Select(p => new ProductDto.Index
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
            })
            .ToListAsync(ctx);
        
        return Result.Success(new ProductResponse.Index
        {
            Products = products,
            TotalCount = totalCount
        });
    }
}