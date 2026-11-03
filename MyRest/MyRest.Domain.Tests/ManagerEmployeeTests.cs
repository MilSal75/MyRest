using MyRest.Domain.Entities;
using MyRest.Domain.Enums;
using MyRest.Domain.Exceptions;
using MyRest.Domain.ValueObjects;
using Xunit;

namespace MyRest.Domain.Tests;

public class ManagerEmployeeTests
{
    // Проверка, что при найме сотрудник связывается с менеджером и попадает в его список.
    [Fact]
    public void HireEmployee_ValidData_CreatesActiveEmployeeLinkedToManager()
    {
        var manager = TestDataFactory.CreateManager();

        var employee = manager.HireEmployee(
            new PersonName("Олег"),
            new PersonName("Петров"),
            new PhoneNumber("79007654321"));

        Assert.Equal(EmployeeStatus.Active, employee.Status);
        Assert.Equal(manager.Id, employee.ManagerId);
        Assert.Equal(manager, employee.Manager);
        Assert.Contains(employee, manager.Employees);
    }

    // Проверка, что нельзя нанять сотрудника без имени.
    [Fact]
    public void HireEmployee_NullFirstName_Throws()
    {
        var manager = TestDataFactory.CreateManager();

        Assert.Throws<ArgumentNullException>(() => manager.HireEmployee(
            null!, new PersonName("Петров"), new PhoneNumber("79007654321")));
        Assert.Empty(manager.Employees);
    }

    // Проверка, что менеджер увольняет своего активного сотрудника.
    [Fact]
    public void FireEmployee_OwnActiveEmployee_FiresAndReturnsTrue()
    {
        var (manager, employee) = TestDataFactory.CreateManagerWithEmployee();

        var result = manager.FireEmployee(employee);

        Assert.True(result);
        Assert.Equal(EmployeeStatus.Fired, employee.Status);
    }

    // Проверка, что повторное увольнение возвращает false.
    [Fact]
    public void FireEmployee_AlreadyFired_ReturnsFalse()
    {
        var (manager, employee) = TestDataFactory.CreateManagerWithEmployee();
        manager.FireEmployee(employee);

        var result = manager.FireEmployee(employee);

        Assert.False(result);
        Assert.Equal(EmployeeStatus.Fired, employee.Status);
    }

    // Проверка, что менеджер не может уволить чужого сотрудника.
    [Fact]
    public void FireEmployee_EmployeeOfAnotherManager_Throws()
    {
        var firstManager = TestDataFactory.CreateManager("Анна");
        var secondManager = TestDataFactory.CreateManager("Ирина");
        var employee = secondManager.HireEmployee(
            new PersonName("Олег"),
            new PersonName("Петров"),
            new PhoneNumber("79007654321"));

        Assert.Throws<EmployeeNotBelongManagerException>(() => firstManager.FireEmployee(employee));
        Assert.Equal(EmployeeStatus.Active, employee.Status);
    }

    // Проверка, что нельзя уволить несуществующего сотрудника.
    [Fact]
    public void FireEmployee_NullEmployee_Throws()
    {
        var manager = TestDataFactory.CreateManager();

        Assert.Throws<ArgumentNullException>(() => manager.FireEmployee(null!));
    }

    // Проверка, что активный сотрудник может создать заявку на отпуск.
    [Fact]
    public void RequestVacation_ActiveEmployee_CreatesRequestedVacation()
    {
        var (_, employee) = TestDataFactory.CreateManagerWithEmployee();
        var start = new DateOnly(2026, 12, 1);
        var end = new DateOnly(2026, 12, 14);

        var vacation = employee.RequestVacation(start, end);

        Assert.Equal(employee, vacation.Employee);
        Assert.Equal(start, vacation.StartDate);
        Assert.Equal(end, vacation.EndDate);
        Assert.Equal(VacationStatus.Requested, vacation.Status);
        Assert.Contains(vacation, employee.Vacations);
    }

    // Проверка, что уволенный сотрудник не может запросить отпуск.
    [Fact]
    public void RequestVacation_FiredEmployee_Throws()
    {
        var (manager, employee) = TestDataFactory.CreateManagerWithEmployee();
        manager.FireEmployee(employee);

        Assert.Throws<InvalidOperationException>(() => employee.RequestVacation(
            new DateOnly(2026, 12, 1), new DateOnly(2026, 12, 14)));
    }
}
