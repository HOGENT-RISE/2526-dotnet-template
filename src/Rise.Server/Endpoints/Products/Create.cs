using Rise.Shared.Products;
namespace Rise.Server.Endpoints.Products;

public class Create(IProductService productService) : Endpoint<ProductRequest.Create, Result<ProductResponse.Create>>
{
    public override void Configure()
    {
        Post("/api/products");
        AllowAnonymous();
    }

    public override Task<Result<ProductResponse.Create>> ExecuteAsync(ProductRequest.Create req, CancellationToken ctx)
    {
        return productService.CreateAsync(req, ctx);
    }
}