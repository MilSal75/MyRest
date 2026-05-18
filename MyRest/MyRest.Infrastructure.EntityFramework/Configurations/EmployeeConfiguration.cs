using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRest.Domain.Entities;

namespace MyRest.Infrastructure.EntityFramework.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Status).IsRequired();

        builder.OwnsOne(e => e.FirstName, nameBuilder =>
        {
            nameBuilder.Property(n => n.Value).HasColumnName("FirstName").IsRequired();
        });

        builder.OwnsOne(e => e.LastName, nameBuilder =>
        {
            nameBuilder.Property(n => n.Value).HasColumnName("LastName").IsRequired();
        });

        builder.OwnsOne(e => e.Phone, phoneBuilder =>
        {
            phoneBuilder.Property(p => p.Value).HasColumnName("Phone").IsRequired();
        });

        builder.HasOne(e => e.Manager)
            .WithMany(m => m.Employees)
            .HasForeignKey(e => e.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}