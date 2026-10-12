using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// namespace pls
namespace infrastructure.src.persistence;
public static class DBDependency
{
    public static void ConfigureDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure your database dependencies here
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
    }
}