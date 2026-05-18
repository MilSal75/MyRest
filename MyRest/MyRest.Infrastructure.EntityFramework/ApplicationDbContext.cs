using Microsoft.EntityFrameworkCore;
using MyRest.Domain.Entities;

namespace MyRest.Infrastructure.EntityFramework;

public class ApplicationDbContext : DbContext
{
    public DbSet<Manager> Managers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<ShiftAssignment> ShiftAssignments { get; set; }
    public DbSet<Vacation> Vacations { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    { 
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}