using MyRest.Domain.Base;
using MyRest.Domain.ValueObjects;
using MyRest.Domain.Enums;
using MyRest.Domain.Exceptions;

namespace MyRest.Domain.Entities;

public class Employee : Entity<Guid>
{
    public PersonName FirstName { get; private set; } = default!;
    public PersonName LastName { get; private set; } = default!;
    public PhoneNumber Phone { get; private set; } = default!;
    public EmployeeStatus Status { get; private set; } = EmployeeStatus.Active;
    public Manager Manager { get; private set; } = default!;
    public Guid ManagerId { get; private set; }

    private readonly List<ShiftAssignment> _shifts = new();
    public IReadOnlyCollection<ShiftAssignment> Shifts => _shifts.AsReadOnly();

    private readonly List<Vacation> _vacations = new();
    public IReadOnlyCollection<Vacation> Vacations => _vacations.AsReadOnly();

    private Employee() : base(Guid.NewGuid()) { }

    public Employee(Guid id, PersonName firstName, PersonName lastName, PhoneNumber phone, Manager manager)
        : base(id)
    {
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        Phone = phone ?? throw new ArgumentNullException(nameof(phone));
        Manager = manager ?? throw new ArgumentNullException(nameof(manager));
        ManagerId = manager.Id;
    }

    internal bool Fire(Manager manager)
    {
        if (manager == null)
            throw new ArgumentNullException(nameof(manager));

        if (ManagerId != manager.Id)
            throw new EmployeeNotBelongManagerException(manager, this);

        if (Status == EmployeeStatus.Fired)
            return false;

        Status = EmployeeStatus.Fired;
        return true;
    }

    public Vacation RequestVacation(DateOnly start, DateOnly end)
    {
        if (Status == EmployeeStatus.Fired)
            throw new InvalidOperationException("Уволенный сотрудник не может запрашивать отпуск.");

        var vacation = new Vacation(Guid.NewGuid(), this, start, end);
        _vacations.Add(vacation);
        return vacation;
    }
}