using MyRest.Domain.ValueObjects.Base;
using MyRest.Domain.ValueObjects.Validators;

namespace MyRest.Domain.ValueObjects;

public class PersonName : ValueObject<string>
{
    private static readonly IValidator<string> _defaultValidator = new PersonNameValidator();

    public PersonName(string name) : base(_defaultValidator, name) { }
}