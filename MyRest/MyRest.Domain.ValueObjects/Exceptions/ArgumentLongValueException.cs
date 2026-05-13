namespace MyRest.Domain.ValueObjects.Exceptions;

public class ArgumentLongValueException(string paramName, int maxLength)
    : Exception($"Параметр {paramName} слишком длинный (максимум {maxLength}).");