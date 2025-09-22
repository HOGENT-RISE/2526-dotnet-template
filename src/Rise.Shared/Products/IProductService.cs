using Rise.Shared.Common;

namespace Rise.Shared.Products;

public interface IProductService
{
    Task<Result<ProductResponse.Create>> CreateAsync(ProductRequest.Create request, CancellationToken ctx);
    Task<Result<ProductResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request, CancellationToken ctx);
}