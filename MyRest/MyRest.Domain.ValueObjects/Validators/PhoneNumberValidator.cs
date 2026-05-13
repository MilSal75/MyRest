using MyRest.Domain.ValueObjects.Exceptions;
using MyRest.Domain.ValueObjects.Base;

namespace MyRest.Domain.ValueObjects.Validators;

public class PhoneNumberValidator : IValidator<string>
{
    public static int MIN_LENGTH => 7;
    public static int MAX_LENGTH => 15;

    public void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullOrWhiteSpaceException(nameof(value));

        if (value.Length < MIN_LENGTH)
            throw new ArgumentShortValueException(nameof(value), MIN_LENGTH);

        if (value.Length > MAX_LENGTH)
            throw new ArgumentLongValueException(nameof(value), MAX_LENGTH);
    }
}