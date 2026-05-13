namespace MyRest.Domain.ValueObjects.Exceptions;

public class ArgumentNullOrWhiteSpaceException(string paramName)
    : ArgumentNullException(paramName, "Значение не может быть пустым.");