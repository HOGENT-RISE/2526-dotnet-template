namespace Rise.Shared.Identity;

/// <summary>
/// Provides a centralized definition of application roles as static properties.
/// This class is used to define role names that are utilized across the application
/// for authentication and authorization purposes.
/// </summary>
public static class AppRoles
{
    public static string Administrator => nameof(Administrator);
    public static string Secretary => nameof(Secretary);
    public static string Technician => nameof(Technician);
}