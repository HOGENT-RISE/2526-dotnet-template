namespace Rise.Domain.Projects;

public class Technician : Entity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string AccountId { get; private set; }

    private readonly List<Project> _projects = [];
    public IReadOnlyCollection<Project> Projects => _projects.AsReadOnly();
    

    /// <summary>
    /// Entity Framework Core Constructor
    /// </summary>
    private Technician()
    {
    }
    
    public Technician(string firstName, string lastName, string accountId)
    {
        FirstName = Guard.Against.NullOrEmpty(firstName);
        LastName = Guard.Against.NullOrEmpty(lastName);
        AccountId = Guard.Against.NullOrEmpty(accountId);
    }
    
    
}