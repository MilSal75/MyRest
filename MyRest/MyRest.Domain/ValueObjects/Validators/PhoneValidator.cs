#nullable disable
using MyRest.Domain.ValueObjects.Base;
using MyRest.Domain.ValueObjects.Exceptions;

namespace MyRest.Domain.ValueObjects.Validators
{
    public class PhoneValidator : IValidator<string>
    {
        public static int MAX_LENGTH => 20;

        public void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidPhoneException("Телефон не может быть пустым!", nameof(value));

            if (value.Length > MAX_LENGTH)
                throw new InvalidPhoneException($"Телефон не может быть длиннее {MAX_LENGTH} символов!", nameof(value));

            foreach (char c in value)
            {
                if (!char.IsDigit(c) && c != '+' && c != '-' && c != ' ')
                    throw new InvalidPhoneException("Телефон может содержать только цифры, +, - и пробелы", nameof(value));
            }
        }
    }
}