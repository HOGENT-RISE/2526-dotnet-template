namespace Rise.Shared.Products;

public static class ProductDto
{
    public class Index
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}