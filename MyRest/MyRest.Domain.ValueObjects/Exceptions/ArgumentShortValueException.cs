namespace MyRest.Domain.ValueObjects.Exceptions;

public class ArgumentShortValueException(string paramName, int minLength)
    : Exception($"Параметр {paramName} слишком короткий (минимум {minLength}).");