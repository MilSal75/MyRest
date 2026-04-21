using Microsoft.EntityFrameworkCore;
using MyRest.Domain.Entities;
using System.Reflection;

namespace MyRest.Infrastructure;

public class ApplicationDbContext : DbContext
{
    // Это таблицы в бд
    public DbSet<Manager> Managers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<ShiftAssignment> ShiftAssignments { get; set; }
    public DbSet<Vacation> Vacations { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // найдет все настройки из папки Configurations и применит их
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}