namespace Rise.Domain.Products;

public class Product : Entity
{
    private string _name = string.Empty;
    public required string Name
    {
        get => _name;
        set => _name = Guard.Against.NullOrEmpty(value);
    }

    private string _description = string.Empty;
    public string Description
    {
        get => _description;
        set => _description = Guard.Against.NullOrEmpty(value);
    }
}