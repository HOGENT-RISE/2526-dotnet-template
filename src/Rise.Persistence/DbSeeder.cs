using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rise.Domain.Products;
using Rise.Domain.Projects;

namespace Rise.Persistence;
/// <summary>
/// Seeds the database
/// </summary>
/// <param name="dbContext"></param>
/// <param name="roleManager"></param>
/// <param name="userManager"></param>
public class DbSeeder(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
{
    const string PasswordDefault = "A1b2C3!";
    public async Task SeedAsync()
    {
        await RolesAsync();
        await UsersAsync();
        await ProductsAsync();
        await ProjectsAsync();
    }

    private async Task RolesAsync()
    {
        if (dbContext.Roles.Any())
            return;

        await roleManager.CreateAsync(new IdentityRole("Administrator"));
        await roleManager.CreateAsync(new IdentityRole("Secretary"));
        await roleManager.CreateAsync(new IdentityRole("Technician"));
    }
    
    private async Task  UsersAsync()
    {
        if (dbContext.Users.Any())
            return;
        
        await dbContext.Roles.ToListAsync();

        var admin = new IdentityUser
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            EmailConfirmed = true,
        };
        await userManager.CreateAsync(admin, PasswordDefault);
        
        var secretary = new IdentityUser
        {
            UserName = "secretary@example.com",
            Email = "secretary@example.com",
            EmailConfirmed = true,
        };
        await userManager.CreateAsync(secretary, PasswordDefault);
        
        var technicianAccount1 = new IdentityUser
        {
            UserName = "technician1@example.com",
            Email = "technician1@example.com",
            EmailConfirmed = true,
        };
        await userManager.CreateAsync(technicianAccount1, PasswordDefault);
        
        var technicianAccount2 = new IdentityUser
        {
            UserName = "technician2@example.com",
            Email = "technician2@example.com",
            EmailConfirmed = true,
        };
        await userManager.CreateAsync(technicianAccount2, PasswordDefault);
                
        var user = new IdentityUser
        {
            UserName = "user@example.com",
            Email = "user@example.com",
            EmailConfirmed = true,
        };
        await userManager.CreateAsync(user, PasswordDefault);
        
        await userManager.AddToRoleAsync(admin, "Administrator");
        await userManager.AddToRoleAsync(secretary, "Secretary");
        await userManager.AddToRoleAsync(technicianAccount1, "Technician");
        await userManager.AddToRoleAsync(technicianAccount2, "Technician");

        dbContext.Technicians.AddRange(
            new Technician("Tech 1", "Awesome", technicianAccount1.Id),
            new Technician("Tech 2", "Less Awesome", technicianAccount2.Id));
        
        await dbContext.SaveChangesAsync();
    }
    

    
    private async Task  ProductsAsync()
    {
        if (dbContext.Products.Any())
            return;
        
        dbContext.Products.AddRange(
            new Product("Laptop", "15-inch display, 16GB RAM"),
            new Product("Smartphone", "6.5-inch screen, 128GB storage"),
            new Product("Headphones", "Wireless noise-cancelling"),
            new Product("Keyboard", "Mechanical RGB backlit"),
            new Product("Mouse", "Ergonomic wireless mouse"),
            new Product("Monitor", "27-inch 4K UHD display"),
            new Product("Printer", "All-in-one inkjet printer"),
            new Product("Camera", "Mirrorless 24MP with 4K video"),
            new Product("Smartwatch", "Heart rate monitor, GPS"),
            new Product("Speaker", "Bluetooth portable speaker")
        );

        await dbContext.SaveChangesAsync();
    }
    
    private async Task  ProjectsAsync()
    {
        if (dbContext.Projects.Any())
            return;
        
        var technicians = await dbContext.Technicians.ToListAsync();
        if (!technicians.Any())
            return;

        var rnd = new Random();
        
        var projects = new List<Project>
        {
            new("Website Redesign", technicians[rnd.Next(technicians.Count)]),
            new("Mobile App Development", technicians[rnd.Next(technicians.Count)]),
            new("Database Migration", technicians[rnd.Next(technicians.Count)])
        };

        dbContext.Projects.AddRange(projects);
        await dbContext.SaveChangesAsync();
    }
}