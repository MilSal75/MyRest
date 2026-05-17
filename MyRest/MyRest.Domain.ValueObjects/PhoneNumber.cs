using MyRest.Domain.ValueObjects.Base;
using MyRest.Domain.ValueObjects.Validators;

namespace MyRest.Domain.ValueObjects;

public class PhoneNumber : ValueObject<string>
{
    private static readonly IValidator<string> _defaultValidator = new PhoneNumberValidator();

    public PhoneNumber(string phone) : this(_defaultValidator, phone)
    {
    }

    public PhoneNumber(IValidator<string> validator, string phone) : base(validator, phone) { }
}