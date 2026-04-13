using MyRest.Domain.ValueObjects.Base;
using MyRest.Domain.ValueObjects.Exceptions;
using System.Text.RegularExpressions;

namespace MyRest.Domain.ValueObjects.Validators;

public class PhoneNumberValidator : IValidator<string>
{
    public void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidPhoneNumberException("Номер телефона не может быть пустым!", nameof(value));

        // Проверка: номер может начинаться с плюса и содержать от 10 до 15 цифр
        if (!Regex.IsMatch(value, @"^\+?[0-9]{10,15}$"))
            throw new InvalidPhoneNumberException("Неверный формат номера телефона!", nameof(value));
    }
}