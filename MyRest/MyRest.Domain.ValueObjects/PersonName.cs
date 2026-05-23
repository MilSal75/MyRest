using MyRest.Domain.ValueObjects.Base;
using MyRest.Domain.ValueObjects.Validators;

namespace MyRest.Domain.ValueObjects;

public class PersonName : ValueObject<string>
{
    protected PersonName() { }

    public PersonName(string value) : base(new PersonNameValidator(), value) { }
}