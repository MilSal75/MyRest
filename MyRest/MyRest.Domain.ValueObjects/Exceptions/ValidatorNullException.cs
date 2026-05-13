using System;

namespace MyRest.Domain.ValueObjects.Exceptions;

public class ValidatorNullException(string paramName)
    : ArgumentNullException(paramName, "Валидатор не может быть null.");