using ECommerce.Data.Concrete.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ECommerce.WebAPI.ContextFactory
{
    public class ShopContextFactory : IDesignTimeDbContextFactory<ShopContext>
    {
        public ShopContext CreateDbContext(string[] args)
        {
            //configuration builder
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            //DbContextOptionsBuilder

            var builder = new DbContextOptionsBuilder<ShopContext>()
                .UseSqlServer(configuration.GetConnectionString("MSSQLConnection"),
                prj => prj.MigrationsAssembly("ECommerce.WebAPI"));

            return new ShopContext(builder.Options);
        }
    }
}
