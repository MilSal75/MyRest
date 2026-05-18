using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRest.Domain.Entities;

namespace MyRest.Infrastructure.EntityFramework.Configurations;

public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {
        builder.ToTable("Managers");
        builder.HasKey(m => m.Id);

        builder.OwnsOne(m => m.FirstName, nameBuilder =>
        {
            nameBuilder.Property(n => n.Value).HasColumnName("FirstName").IsRequired();
        });

        builder.OwnsOne(m => m.LastName, nameBuilder =>
        {
            nameBuilder.Property(n => n.Value).HasColumnName("LastName").IsRequired();
        });

        builder.OwnsOne(m => m.Phone, phoneBuilder =>
        {
            phoneBuilder.Property(p => p.Value).HasColumnName("Phone").IsRequired();
        });

        builder.HasMany(m => m.Employees)
            .WithOne(e => e.Manager)
            .HasForeignKey(e => e.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}