#nullable disable
using System;

namespace MyRest.Domain.ValueObjects.Exceptions
{
    public class ValidatorNullException : ArgumentNullException
    {
        public ValidatorNullException(string paramName) : base(paramName, "Валидатор не может быть null") { }
    }
}