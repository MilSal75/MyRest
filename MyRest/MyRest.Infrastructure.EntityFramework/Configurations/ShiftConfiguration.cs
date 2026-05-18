using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRest.Domain.Entities;

namespace MyRest.Infrastructure.EntityFramework.Configurations;

public class ShiftConfiguration : IEntityTypeConfiguration<Shift>
{
    public void Configure(EntityTypeBuilder<Shift> builder)
    {
        builder.ToTable("Shifts");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.ShiftDate).IsRequired();

        builder.OwnsOne(s => s.Duration, durationBuilder =>
        {
            durationBuilder.Property(d => d.Value).HasColumnName("Duration").IsRequired();
        });

        builder.HasOne(s => s.Manager)
            .WithMany()
            .HasForeignKey("ManagerId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}