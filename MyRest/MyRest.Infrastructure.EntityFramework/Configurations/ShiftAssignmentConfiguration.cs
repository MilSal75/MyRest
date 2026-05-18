using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRest.Domain.Entities;

namespace MyRest.Infrastructure.EntityFramework.Configurations;

public class ShiftAssignmentConfiguration : IEntityTypeConfiguration<ShiftAssignment>
{
    public void Configure(EntityTypeBuilder<ShiftAssignment> builder)
    {
        builder.ToTable("ShiftAssignments");
        builder.HasKey(sa => sa.Id);

        builder.Property(sa => sa.Status).IsRequired();

        builder.HasOne(sa => sa.Employee)
            .WithMany(e => e.Shifts)
            .HasForeignKey("EmployeeId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sa => sa.Shift)
            .WithMany()
            .HasForeignKey("ShiftId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}