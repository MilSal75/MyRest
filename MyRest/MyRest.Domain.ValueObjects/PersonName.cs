using MyRest.Domain.ValueObjects.Base;
using MyRest.Domain.ValueObjects.Validators;

namespace MyRest.Domain.ValueObjects;

public class PersonName(string value) : ValueObject<string>(new PersonNameValidator(), value);