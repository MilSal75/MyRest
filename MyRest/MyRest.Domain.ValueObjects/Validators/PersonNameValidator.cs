using MyRest.Domain.ValueObjects.Base;
using MyRest.Domain.ValueObjects.Exceptions;
using System.Text.RegularExpressions;

namespace MyRest.Domain.ValueObjects.Validators;

public class PersonNameValidator : IValidator<string>
{
    public void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationDomainException("Имя или фамилия не могут быть пустыми.", nameof(value));

        if (value.Length < 2 || value.Length > 50)
            throw new ValidationDomainException("Длина имени должна быть от 2 до 50 символов.", nameof(value));

        // Разрешаем только русские/английские буквы, пробелы и дефисы (например, для двойных имен вроде "Анна-Мария")
        if (!Regex.IsMatch(value, @"^[а-яА-ЯёЁa-zA-Z\s\-]+$"))
            throw new ValidationDomainException("Имя может содержать только буквы, пробелы и дефисы.", nameof(value));
    }
}