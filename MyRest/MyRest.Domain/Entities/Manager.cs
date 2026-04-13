using MyRest.Domain.Enums;
using MyRest.Domain.Exceptions;
using MyRest.Domain.ValueObjects;

namespace MyRest.Domain.Entities;

public class Manager
{
    public Guid Id { get; } = Guid.NewGuid();

    // Используем Value Object
    public PersonName FirstName { get; private set; }
    public PersonName LastName { get; private set; }
    public PhoneNumber Phone { get; private set; }
    public UserStatus Status { get; private set; } = UserStatus.Active;

    // Конструктор обновлен
    public Manager(PersonName firstName, PersonName lastName, PhoneNumber phone)
    {
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
    }

    // Метод найма теперь требует PersonName
    public Employee HireEmployee(PersonName firstName, PersonName lastName, PhoneNumber phone)
    {
        return new Employee(this.Id, firstName, lastName, phone);
    }

    public void FireEmployee(Employee employee)
    {
        if (employee.ManagerId != this.Id)
            throw new InvalidEntityStateException("Этот сотрудник не является вашим подчиненным.");

        employee.Fire();
    }

    public Shift CreateShift(DateOnly date, TimeOnly startTime, decimal durationHours)
    {
        if (Status != UserStatus.Active)
            throw new InvalidEntityStateException("Только активный менеджер может создавать смены.");

        return new Shift(this.Id, date, startTime, durationHours);
    }

    public ShiftAssignment AssignShift(Employee employee, Shift shift)
    {
        if (employee.ManagerId != this.Id)
            throw new InvalidEntityStateException("Нельзя назначать чужого сотрудника.");
        if (employee.Status != UserStatus.Active)
            throw new InvalidEntityStateException("Нельзя назначить на смену неактивного сотрудника.");

        var assignment = new ShiftAssignment(employee.Id, shift.Id);
        employee.AddAssignment(assignment);
        shift.AddAssignment(assignment);

        return assignment;
    }

    public void ResolveVacation(Employee employee, Vacation vacation, bool isApproved)
    {
        if (employee.ManagerId != this.Id)
            throw new InvalidEntityStateException("Сотрудник вам не подчиняется.");

        if (isApproved)
            vacation.Approve();
        else
            vacation.Reject();
    }
}