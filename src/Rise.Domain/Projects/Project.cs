using Ardalis.Result;

namespace Rise.Domain.Projects;

public class Project : Entity
{
    public string Name { get; private set; }
    public Technician Technician { get; private set; }

    /// <summary>
    /// Entity Framework Core Constructor
    /// </summary>
    private Project()
    {
    }
    
    public Project(string name, Technician technician)
    {
        Name = Guard.Against.NullOrEmpty(name);
        Technician = Guard.Against.Null(technician);
    }
    
    public bool CanBeEditedBy(Technician technician)
    {
        return Technician == technician; // due to Entity (comparision via ID)
    }

    public void Edit(string name)
    {
        Name = Guard.Against.NullOrEmpty(name);       
    }
}