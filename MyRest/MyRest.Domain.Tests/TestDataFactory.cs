using MyRest.Domain.Entities;
using MyRest.Domain.ValueObjects;

namespace MyRest.Domain.Tests;

internal static class TestDataFactory
{
    internal static Manager CreateManager(string firstName = "Анна") =>
        new(Guid.NewGuid(), new PersonName(firstName), new PersonName("Иванова"), new PhoneNumber("79001234567"));

    internal static (Manager Manager, Employee Employee) CreateManagerWithEmployee()
    {
        var manager = CreateManager();
        var employee = manager.HireEmployee(
            new PersonName("Олег"),
            new PersonName("Петров"),
            new PhoneNumber("79007654321"));

        return (manager, employee);
    }

    internal static (Manager Manager, Employee Employee, Shift Shift) CreateShiftData()
    {
        var (manager, employee) = CreateManagerWithEmployee();
        var shift = new Shift(manager, new DateTime(2026, 11, 2, 9, 0, 0), new ShiftDuration(8));

        return (manager, employee, shift);
    }
}
