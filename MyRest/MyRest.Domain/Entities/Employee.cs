using MyRest.Domain.ValueObjects;
using MyRest.Domain.Enums;
using System;
using System.Collections.Generic;

namespace MyRest.Domain.Entities;

public class Employee
{
    public Guid Id { get; } = Guid.NewGuid();
    public PersonName FirstName { get; private set; }
    public PersonName LastName { get; private set; }
    public PhoneNumber Phone { get; private set; }

    public Manager Manager { get; private set; }
    public UserStatus Status { get; private set; } = UserStatus.Active;

    private List<Vacation> _vacations = new List<Vacation>();
    public IReadOnlyCollection<Vacation> Vacations => _vacations.AsReadOnly();

    private Employee() { }

    public Employee(Manager manager, string fName, string lName, string phone)
    {
        Manager = manager ?? throw new ArgumentNullException(nameof(manager));
        FirstName = new PersonName(fName);
        LastName = new PersonName(lName);
        Phone = new PhoneNumber(phone);
    }

    public void Fire() => Status = UserStatus.Fired;

    public Vacation RequestVacation(DateTime start, DateTime end)
    {
        var vacation = new Vacation(this, start, end);
        _vacations.Add(vacation);
        return vacation;
    }
}