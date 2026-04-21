using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRest.Domain.Entities;
using MyRest.Domain.ValueObjects;

namespace MyRest.Infrastructure.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");
        builder.HasKey(e => e.Id);

        // Настройка связи с Менеджером
        builder.HasOne<Manager>()
            .WithMany()
            .HasForeignKey(e => e.ManagerId)
            .OnDelete(DeleteBehavior.Restrict); // Запрещаем удалять менеджера, если у него есть сотрудники

        // Value Objects
        builder.Property(e => e.FirstName)
            .HasConversion(v => v.Value, v => new PersonName(v))
            .HasColumnName("FirstName").HasMaxLength(50).IsRequired();

        builder.Property(e => e.LastName)
            .HasConversion(v => v.Value, v => new PersonName(v))
            .HasColumnName("LastName").HasMaxLength(50).IsRequired();

        builder.Property(e => e.Phone)
            .HasConversion(v => v.Value, v => new PhoneNumber(v))
            .HasColumnName("Phone").HasMaxLength(15).IsRequired();

        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(20).IsRequired();

        // Настройка приватных коллекций (чтобы EF Core мог в них записывать данные)
        builder.Metadata.FindNavigation(nameof(Employee.Vacations))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Metadata.FindNavigation(nameof(Employee.Assignments))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}