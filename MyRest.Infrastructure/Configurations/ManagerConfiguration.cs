using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRest.Domain.Entities;
using MyRest.Domain.ValueObjects;

namespace MyRest.Infrastructure.Configurations;

public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {
        builder.ToTable("Managers");

        builder.HasKey(m => m.Id);

        // Конвертируем Value Object (PersonName) в обычную строку для БД
        builder.Property(m => m.FirstName)
            .HasConversion(
                name => name.Value,            // Как сохранять в БД (берем строку из объекта)
                value => new PersonName(value)) // Как читать из БД (создаем объект из строки)
            .HasColumnName("FirstName")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(m => m.LastName)
            .HasConversion(
                name => name.Value,
                value => new PersonName(value))
            .HasColumnName("LastName")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(m => m.Phone)
            .HasConversion(
                phone => phone.Value,
                value => new PhoneNumber(value))
            .HasColumnName("Phone")
            .HasMaxLength(15)
            .IsRequired();

        // Конвертация Enum статуса в строку
        builder.Property(m => m.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
    }
}