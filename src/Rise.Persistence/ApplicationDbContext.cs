using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rise.Domain.Products;
using Rise.Domain.Projects;

namespace Rise.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts) : IdentityDbContext<IdentityUser>(opts)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Technician> Technicians => Set<Technician>();
  
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // All columns in the database have a maxlength of 4000.
        // in NVARACHAR 4000 is the maximum length that can be indexed by a database.
        // Some columns need more length, but these can be set on the configuration level for that Entity in particular.
        configurationBuilder.Properties<string>().HaveMaxLength(4_000);
        // All decimals columns should have 2 digits after the comma
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Applying all types of IEntityTypeConfiguration in the Persistence project.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
