using MyRest.Domain.ValueObjects;
using MyRest.Domain.ValueObjects.Exceptions;
using Xunit;

namespace MyRest.Domain.Tests;

public class ValueObjectTests
{
    // Проверка, что корректное имя сохраняется в объекте.
    [Fact]
    public void PersonName_ValidValue_StoresValue()
    {
        var name = new PersonName("Анна");

        Assert.Equal("Анна", name.Value);
        Assert.Equal("Анна", name.ToString());
    }

    // Проверка, что имя не может быть пустым или состоять из пробелов.
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void PersonName_NullOrWhitespace_Throws(string? value)
    {
        Assert.Throws<ArgumentNullOrWhiteSpaceException>(() => new PersonName(value!));
    }

    // Проверка, что имя короче двух символов не принимается.
    [Fact]
    public void PersonName_TooShort_Throws()
    {
        Assert.Throws<ArgumentShortValueException>(() => new PersonName("А"));
    }

    // Проверка, что имя длиннее 50 символов не принимается.
    [Fact]
    public void PersonName_TooLong_Throws()
    {
        Assert.Throws<ArgumentLongValueException>(() => new PersonName(new string('А', 51)));
    }

    // Проверка, что корректный номер телефона сохраняется в объекте.
    [Fact]
    public void PhoneNumber_ValidValue_StoresValue()
    {
        var phone = new PhoneNumber("79001234567");

        Assert.Equal("79001234567", phone.Value);
    }

    // Проверка, что номер телефона не может быть пустым или состоять из пробелов.
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void PhoneNumber_NullOrWhitespace_Throws(string? value)
    {
        Assert.Throws<ArgumentNullOrWhiteSpaceException>(() => new PhoneNumber(value!));
    }

    // Проверка, что номер короче 7 или длиннее 15 символов не принимается.
    [Theory]
    [InlineData("123456", typeof(ArgumentShortValueException))]
    [InlineData("1234567890123456", typeof(ArgumentLongValueException))]
    public void PhoneNumber_OutsideAllowedLength_Throws(string value, Type exceptionType)
    {
        Assert.Throws(exceptionType, () => new PhoneNumber(value));
    }

    // Проверка, что номер телефона нельзя создать без валидатора.
    [Fact]
    public void PhoneNumber_NullValidator_Throws()
    {
        Assert.Throws<ValidatorNullException>(() => new PhoneNumber(null!, "79001234567"));
    }

    // Проверка, что длительность смены от 1 до 12 часов принимается.
    [Theory]
    [InlineData(1.0)]
    [InlineData(8.0)]
    [InlineData(12.0)]
    public void ShiftDuration_AllowedBoundary_StoresValue(double hours)
    {
        var duration = new ShiftDuration(hours);

        Assert.Equal(hours, duration.Value);
    }

    // Проверка, что длительность смены меньше 1 или больше 12 часов не принимается.
    [Theory]
    [InlineData(0.99)]
    [InlineData(12.01)]
    public void ShiftDuration_OutsideAllowedRange_Throws(double hours)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new ShiftDuration(hours));
    }

    // Проверка, что два одинаковых имени считаются равными.
    [Fact]
    public void ValueObjects_SameTypeAndValue_AreEqual()
    {
        var first = new PersonName("Анна");
        var second = new PersonName("Анна");

        Assert.Equal(first, second);
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }

    // Проверка, что имя и номер телефона не равны друг другу.
    [Fact]
    public void ValueObjects_DifferentConcreteTypes_AreNotEqual()
    {
        var name = new PersonName("1234567");
        var phone = new PhoneNumber("1234567");

        Assert.False(name.Equals(phone));
    }
}
