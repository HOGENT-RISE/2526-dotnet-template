using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Products;
using Rise.Domain.Projects;

namespace Rise.Persistence.Configurations.Projects;

/// <summary>
/// Specific configuration for <see cref="Product"/>.
/// </summary>
internal class ProjectConfiguration : EntityConfiguration<Project>
{
    public override void Configure(EntityTypeBuilder<Project> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(250);
        builder.HasOne(x => x.Technician)
            .WithMany(x => x.Projects)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Other Project configuration here.
    }
}