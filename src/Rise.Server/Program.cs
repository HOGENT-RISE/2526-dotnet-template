using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rise.Persistence;
using Rise.Persistence.Triggers;
using Rise.Server.Identity;
using Rise.Server.Processors;
using Rise.Services;
using Rise.Services.Identity;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting web application");
    var builder = WebApplication.CreateBuilder(args);

    builder.Services
        .AddSerilog((_, lc) => lc.ReadFrom.Configuration(builder.Configuration))
        .AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .Services.AddDbContext<ApplicationDbContext>(o =>
        {
            var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection") ??
                                   throw new InvalidOperationException("Connection string 'DatabaseConnection' not found.");
            o.UseSqlite(connectionString);
            o.EnableDetailedErrors();
            o.EnableSensitiveDataLogging();
            o.UseTriggers(options => options.AddTrigger<EntityBeforeSaveTrigger>());
        })
        .AddHttpContextAccessor()
        .AddScoped<ISessionContextProvider, HttpContextSessionProvider>()
        .AddApplicationServices()
        .AddAuthorization()
        .AddFastEndpoints(o =>
        {
            o.IncludeAbstractValidators = true;
            o.Assemblies = [typeof(Rise.Shared.Products.ProductRequest).Assembly];
        })
        .SwaggerDocument(o =>
        {
            o.DocumentSettings = s =>
            {
                s.Title = "RISE API";
            };
        });

    var app = builder.Build();
    // apply Database migraticons on startup, not so wise in production (Use Generated SQL Scripts) 
    // See: https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli
    if (app.Environment.IsDevelopment())
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var dbSeeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
            // dbContext.Database.EnsureDeleted();

            dbContext.Database.Migrate();
            await dbSeeder.SeedAsync();
        }
    }

    app.UseHttpsRedirection()
        .UseBlazorFrameworkFiles()
        .UseStaticFiles()
        .UseDefaultExceptionHandler()
        .UseAuthentication()
        .UseAuthorization()
        .UseFastEndpoints(o =>
        {
            o.Endpoints.Configurator = ep =>
            {
                ep.DontAutoSendResponse();
                ep.PreProcessor<GlobalRequestLogger>(Order.Before);
                ep.PostProcessor<GlobalResponseSender>(Order.Before);
                ep.PostProcessor<GlobalResponseLogger>(Order.Before);
            };
        })
        .UseSwaggerGen();
    app.MapFallbackToFile("index.html");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
}
finally
{
    Log.CloseAndFlush();
}


