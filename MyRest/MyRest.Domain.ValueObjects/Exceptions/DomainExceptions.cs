using System;

namespace MyRest.Domain.ValueObjects.Exceptions;

public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
}

public class ValidationDomainException : DomainException
{
    public ValidationDomainException(string message, string paramName = "")
        : base($"{message} (Параметр: {paramName})") { }
}

public class InvalidPhoneNumberException : DomainException
{
    public InvalidPhoneNumberException(string message, string paramName = "")
        : base($"{message} (Параметр: {paramName})") { }
}

public class ValidatorNullException : ArgumentNullException
{
    public ValidatorNullException(string paramName) :
        base(paramName, "Валидатор не может быть null")
    { }
}