using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRest.Domain.Entities;

namespace MyRest.Infrastructure.EntityFramework.Configurations;

public class VacationConfiguration : IEntityTypeConfiguration<Vacation>
{
    public void Configure(EntityTypeBuilder<Vacation> builder)
    {
        builder.ToTable("Vacations");
        builder.HasKey(v => v.Id);

        builder.Property(v => v.StartDate).IsRequired();
        builder.Property(v => v.EndDate).IsRequired();
        builder.Property(v => v.Status).IsRequired();

        builder.HasOne(v => v.Employee)
            .WithMany(e => e.Vacations)
            .HasForeignKey("EmployeeId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}