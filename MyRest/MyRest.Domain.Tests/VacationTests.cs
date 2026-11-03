using MyRest.Domain.Entities;
using MyRest.Domain.Enums;
using MyRest.Domain.Exceptions;
using Xunit;

namespace MyRest.Domain.Tests;

public class VacationTests
{
    // Проверка, что дата начала отпуска не может быть позже даты окончания.
    [Fact]
    public void Constructor_StartAfterEnd_Throws()
    {
        var (_, employee) = TestDataFactory.CreateManagerWithEmployee();

        Assert.Throws<ArgumentException>(() => new Vacation(
            Guid.NewGuid(), employee, new DateOnly(2026, 12, 10), new DateOnly(2026, 12, 1)));
    }

    // Проверка, что менеджер может одобрить отпуск своего сотрудника.
    [Fact]
    public void ResolveVacation_OwnEmployeeAndApprove_ApprovesVacation()
    {
        var (manager, employee) = TestDataFactory.CreateManagerWithEmployee();
        var vacation = employee.RequestVacation(new DateOnly(2026, 12, 1), new DateOnly(2026, 12, 14));

        manager.ResolveVacation(employee, vacation, approve: true);

        Assert.Equal(VacationStatus.Approved, vacation.Status);
    }

    // Проверка, что менеджер может отклонить отпуск своего сотрудника.
    [Fact]
    public void ResolveVacation_OwnEmployeeAndReject_RejectsVacation()
    {
        var (manager, employee) = TestDataFactory.CreateManagerWithEmployee();
        var vacation = employee.RequestVacation(new DateOnly(2026, 12, 1), new DateOnly(2026, 12, 14));

        manager.ResolveVacation(employee, vacation, approve: false);

        Assert.Equal(VacationStatus.Rejected, vacation.Status);
    }

    // Проверка, что менеджер не может решить вопрос об отпуске чужого сотрудника.
    [Fact]
    public void ResolveVacation_EmployeeOfAnotherManager_Throws()
    {
        var firstManager = TestDataFactory.CreateManager("Анна");
        var (secondManager, employee) = TestDataFactory.CreateManagerWithEmployee();
        var vacation = employee.RequestVacation(new DateOnly(2026, 12, 1), new DateOnly(2026, 12, 14));

        Assert.Throws<EmployeeNotBelongManagerException>(() =>
            firstManager.ResolveVacation(employee, vacation, approve: true));
        Assert.Equal(VacationStatus.Requested, vacation.Status);
        Assert.NotEqual(firstManager.Id, secondManager.Id);
    }

    // Проверка, что одобренный отпуск нельзя одобрить повторно.
    [Fact]
    public void Approve_AlreadyApprovedVacation_Throws()
    {
        var (manager, employee) = TestDataFactory.CreateManagerWithEmployee();
        var vacation = employee.RequestVacation(new DateOnly(2026, 12, 1), new DateOnly(2026, 12, 14));
        manager.ResolveVacation(employee, vacation, approve: true);

        Assert.Throws<InvalidOperationException>(() => vacation.Approve(manager));
    }

    // Проверка, что чужой менеджер не может одобрить отпуск.
    [Fact]
    public void Approve_AnotherManager_Throws()
    {
        var (_, employee) = TestDataFactory.CreateManagerWithEmployee();
        var otherManager = TestDataFactory.CreateManager("Ирина");
        var vacation = employee.RequestVacation(new DateOnly(2026, 12, 1), new DateOnly(2026, 12, 14));

        Assert.Throws<UnauthorizedAccessException>(() => vacation.Approve(otherManager));
    }

    // Проверка, что отклонённый отпуск нельзя отклонить повторно.
    [Fact]
    public void Reject_AlreadyRejectedVacation_Throws()
    {
        var (manager, employee) = TestDataFactory.CreateManagerWithEmployee();
        var vacation = employee.RequestVacation(new DateOnly(2026, 12, 1), new DateOnly(2026, 12, 14));
        manager.ResolveVacation(employee, vacation, approve: false);

        Assert.Throws<InvalidOperationException>(() => vacation.Reject(manager));
    }

    // Проверка, что отпуск нельзя одобрить без менеджера.
    [Fact]
    public void Approve_NullManager_Throws()
    {
        var (_, employee) = TestDataFactory.CreateManagerWithEmployee();
        var vacation = employee.RequestVacation(new DateOnly(2026, 12, 1), new DateOnly(2026, 12, 14));

        Assert.Throws<ArgumentNullException>(() => vacation.Approve(null!));
    }

    // Проверка, что отпуск нельзя отклонить без менеджера.
    [Fact]
    public void Reject_NullManager_Throws()
    {
        var (_, employee) = TestDataFactory.CreateManagerWithEmployee();
        var vacation = employee.RequestVacation(new DateOnly(2026, 12, 1), new DateOnly(2026, 12, 14));

        Assert.Throws<ArgumentNullException>(() => vacation.Reject(null!));
    }
}
