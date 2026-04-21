using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyRest.Infrastructure;

// Этот класс нужен для консоли разработчика, чтобы она знала, как создать базу
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // Указываем путь к стандартной локальной базе данных Visual Studio
        // Назовем нашу базу данных "MyRestDb"
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MyRestDb;Trusted_Connection=True;MultipleActiveResultSets=true");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}