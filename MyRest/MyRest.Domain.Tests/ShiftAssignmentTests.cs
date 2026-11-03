using MyRest.Domain.Entities;
using MyRest.Domain.Enums;
using MyRest.Domain.ValueObjects;
using Xunit;

namespace MyRest.Domain.Tests;

public class ShiftAssignmentTests
{
    // Проверка, что смена сохраняет менеджера, дату и длительность.
    [Fact]
    public void Shift_ValidData_StoresManagerDateAndDuration()
    {
        var manager = TestDataFactory.CreateManager();
        var date = new DateTime(2026, 11, 2, 9, 0, 0);
        var duration = new ShiftDuration(8);

        var shift = new Shift(manager, date, duration);

        Assert.Equal(manager, shift.Manager);
        Assert.Equal(date, shift.ShiftDate);
        Assert.Equal(duration, shift.Duration);
    }

    // Проверка, что смену нельзя создать без менеджера.
    [Fact]
    public void Shift_NullManager_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new Shift(
            null!, new DateTime(2026, 11, 2), new ShiftDuration(8)));
    }

    // Проверка, что новое назначение на смену получает статус Planned.
    [Fact]
    public void ShiftAssignment_ValidData_StartsPlanned()
    {
        var (_, employee, shift) = TestDataFactory.CreateShiftData();

        var assignment = new ShiftAssignment(Guid.NewGuid(), employee, shift);

        Assert.Equal(employee, assignment.Employee);
        Assert.Equal(shift, assignment.Shift);
        Assert.Equal(AssignmentStatus.Planned, assignment.Status);
    }

    // Проверка, что сотрудник может завершить свою смену.
    [Fact]
    public void Complete_ByEmployee_CompletesAssignment()
    {
        var (_, employee, shift) = TestDataFactory.CreateShiftData();
        var assignment = new ShiftAssignment(Guid.NewGuid(), employee, shift);

        assignment.Complete(employee.Id);

        Assert.Equal(AssignmentStatus.Completed, assignment.Status);
    }

    // Проверка, что менеджер сотрудника может завершить его смену.
    [Fact]
    public void Complete_ByEmployeesManager_CompletesAssignment()
    {
        var (manager, employee, shift) = TestDataFactory.CreateShiftData();
        var assignment = new ShiftAssignment(Guid.NewGuid(), employee, shift);

        assignment.Complete(manager.Id);

        Assert.Equal(AssignmentStatus.Completed, assignment.Status);
    }

    // Проверка, что посторонний пользователь не может завершить смену.
    [Fact]
    public void Complete_ByUnrelatedUser_Throws()
    {
        var (_, employee, shift) = TestDataFactory.CreateShiftData();
        var assignment = new ShiftAssignment(Guid.NewGuid(), employee, shift);

        Assert.Throws<UnauthorizedAccessException>(() => assignment.Complete(Guid.NewGuid()));
        Assert.Equal(AssignmentStatus.Planned, assignment.Status);
    }

    // Проверка, что завершённую смену нельзя завершить повторно.
    [Fact]
    public void Complete_AlreadyCompletedAssignment_Throws()
    {
        var (_, employee, shift) = TestDataFactory.CreateShiftData();
        var assignment = new ShiftAssignment(Guid.NewGuid(), employee, shift);
        assignment.Complete(employee.Id);

        Assert.Throws<InvalidOperationException>(() => assignment.Complete(employee.Id));
    }

    // Проверка, что менеджер может отменить запланированную смену своего сотрудника.
    [Fact]
    public void Cancel_ByEmployeesManager_CancelsAssignment()
    {
        var (manager, employee, shift) = TestDataFactory.CreateShiftData();
        var assignment = new ShiftAssignment(Guid.NewGuid(), employee, shift);

        assignment.Cancel(manager);

        Assert.Equal(AssignmentStatus.Cancelled, assignment.Status);
    }

    // Проверка, что чужой менеджер не может отменить смену.
    [Fact]
    public void Cancel_ByAnotherManager_Throws()
    {
        var (_, employee, shift) = TestDataFactory.CreateShiftData();
        var assignment = new ShiftAssignment(Guid.NewGuid(), employee, shift);
        var otherManager = TestDataFactory.CreateManager("Ирина");

        Assert.Throws<UnauthorizedAccessException>(() => assignment.Cancel(otherManager));
    }

    // Проверка, что выполненную смену нельзя отменить.
    [Fact]
    public void Cancel_CompletedAssignment_Throws()
    {
        var (manager, employee, shift) = TestDataFactory.CreateShiftData();
        var assignment = new ShiftAssignment(Guid.NewGuid(), employee, shift);
        assignment.Complete(employee.Id);

        Assert.Throws<InvalidOperationException>(() => assignment.Cancel(manager));
    }

    // Проверка, что смену нельзя отменить без менеджера.
    [Fact]
    public void Cancel_NullManager_Throws()
    {
        var (_, employee, shift) = TestDataFactory.CreateShiftData();
        var assignment = new ShiftAssignment(Guid.NewGuid(), employee, shift);

        Assert.Throws<ArgumentNullException>(() => assignment.Cancel(null!));
    }
}
