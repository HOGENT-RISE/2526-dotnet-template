namespace Rise.Shared.Products;

public static partial class ProductResponse
{
    public class Index
    {
        public IEnumerable<ProductDto.Index> Products { get; set; } = [];
        public int TotalCount { get; set; }
    }
}

