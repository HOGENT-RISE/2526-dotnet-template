using System.Diagnostics.CodeAnalysis;
using Ardalis.GuardClauses;

namespace Rise.Domain.Products;

public class Product : Entity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    /// <summary>
    /// Entity Framework Core Constructor
    /// </summary>
    private Product()
    {
    }

    public Product(string name, string description) 
    {
        Name = Guard.Against.NullOrEmpty(name);
        Description = Guard.Against.NullOrEmpty(description);
    }
}