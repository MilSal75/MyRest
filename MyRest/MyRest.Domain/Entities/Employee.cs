using MyRest.Domain.Base;
using MyRest.Domain.ValueObjects;
using MyRest.Domain.Enums;

namespace MyRest.Domain.Entities;

public class Employee : Entity<Guid>
{
    private readonly ICollection<Vacation> _vacations = [];
    public Manager Manager { get; private set; } = default!;
    public PersonName FirstName { get; private set; } = default!;
    public PersonName LastName { get; private set; } = default!;
    public PhoneNumber Phone { get; private set; } = default!;
    public UserStatus Status { get; private set; } = UserStatus.Active;

    protected Employee() { }

    public Employee(Manager manager, PersonName firstName, PersonName lastName, PhoneNumber phone)
        : this(Guid.NewGuid(), manager, firstName, lastName, phone) { }

    protected Employee(Guid id, Manager manager, PersonName firstName, PersonName lastName, PhoneNumber phone) : base(id)
    {
        Manager = manager ?? throw new ArgumentNullException(nameof(manager));
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        Phone = phone ?? throw new ArgumentNullException(nameof(phone));
    }

    internal bool Fire()
    {
        if (Status == UserStatus.Fired) return false;
        Status = UserStatus.Fired;
        return true;
    }
}