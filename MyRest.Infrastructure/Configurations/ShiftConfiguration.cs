using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRest.Domain.Entities;

namespace MyRest.Infrastructure.Configurations;

public class ShiftConfiguration : IEntityTypeConfiguration<Shift>
{
    public void Configure(EntityTypeBuilder<Shift> builder)
    {
        builder.ToTable("Shifts");
        builder.HasKey(s => s.Id);

        builder.HasOne<Manager>()
            .WithMany()
            .HasForeignKey(s => s.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(s => s.DurationHours)
            .HasColumnType("decimal(4,1)") // Например: 12.5 часов
            .IsRequired();

        builder.Metadata.FindNavigation(nameof(Shift.Assignments))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}