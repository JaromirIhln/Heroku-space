using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace WorkDays.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Načte konfiguraci z appsettings.json v rootu solution
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
