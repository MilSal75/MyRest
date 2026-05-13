using MyRest.Domain.ValueObjects.Base;
using MyRest.Domain.ValueObjects.Validators;

namespace MyRest.Domain.ValueObjects;

public class ShiftDuration(double value) : ValueObject<double>(new ShiftDurationValidator(), value);

public class ShiftDurationValidator : IValidator<double>
{
    public void Validate(double value)
    {
        if (value < 1.0 || value > 12.0)
            throw new ArgumentOutOfRangeException(nameof(value), "Смена должна быть от 1 до 12 часов.");
    }
}