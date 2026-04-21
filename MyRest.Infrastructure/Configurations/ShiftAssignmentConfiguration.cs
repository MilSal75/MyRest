using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRest.Domain.Entities;

namespace MyRest.Infrastructure.Configurations;

public class ShiftAssignmentConfiguration : IEntityTypeConfiguration<ShiftAssignment>
{
    public void Configure(EntityTypeBuilder<ShiftAssignment> builder)
    {
        builder.ToTable("ShiftAssignments");
        builder.HasKey(sa => sa.Id);

        // Связь с Сотрудником
        builder.HasOne<Employee>()
            .WithMany(e => e.Assignments)
            .HasForeignKey(sa => sa.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade); // Если удалят сотрудника, его назначения тоже удалятся

        // Связь со Сменой
        builder.HasOne<Shift>()
            .WithMany(s => s.Assignments)
            .HasForeignKey(sa => sa.ShiftId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(sa => sa.Status)
            .HasConversion<string>()
            .HasMaxLength(20).IsRequired();
    }
}