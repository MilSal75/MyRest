#nullable disable
using System;

namespace MyRest.Domain.ValueObjects.Exceptions
{
    public class InvalidPhoneException : ArgumentException
    {
        public InvalidPhoneException(string message, string paramName) : base(message, paramName) { }
    }
}