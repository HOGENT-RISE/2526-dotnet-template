using Rise.Shared.Common;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public class FakeProductService : IProductService
{
    private readonly List<ProductDto.Index> _products = new()
    {
        new() { Id = 1, Name = "Laptop", Description = "High-performance laptop" },
        new() { Id = 2, Name = "Smartphone", Description = "Latest model smartphone" },
        new() { Id = 3, Name = "Headphones", Description = "Noise-cancelling headphones" }
    };

    public Task<Result<ProductResponse.Create>> CreateAsync(ProductRequest.Create request, CancellationToken ctx = default)
    {
        var newProduct = new ProductDto.Index
        {
            Id = _products.Count + 1,
            Name = request.Name ?? "Unnamed Product",
            Description = request.Description ?? string.Empty
        };

        _products.Add(newProduct);

        var result = Result<ProductResponse.Create>.Success(new ProductResponse.Create
        {
            ProductId = newProduct.Id
        });

        return Task.FromResult(result);
    }

    public Task<Result<ProductResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request, CancellationToken ctx = default)
    {
        var filtered = string.IsNullOrWhiteSpace(request.SearchTerm)
            ? _products
            : _products.Where(p => p.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                                   p.Description.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

        var paged = filtered
            .Skip(request.Skip)
            .Take(request.Take)
            .ToList();

        var index = new ProductResponse.Index
        {
            Products = paged,
            TotalCount = filtered.Count
        };

        var result = Result<ProductResponse.Index>.Success(index);
        return Task.FromResult(result);
    }
}
