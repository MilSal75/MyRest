using System;
using System.Collections.Generic;
using System.Linq;
using MyRest.Domain.Base;
using MyRest.Domain.ValueObjects;
using MyRest.Domain.Enums;
using MyRest.Domain.Exceptions;

namespace MyRest.Domain.Entities;

public class Employee : Entity<Guid>
{
    public PersonName FirstName { get; private set; }
    public PersonName LastName { get; private set; }
    public PhoneNumber Phone { get; private set; }
    public EmployeeStatus Status { get; private set; }
    public Manager Manager { get; private set; }
    public Guid ManagerId { get; private set; }


    public DateOnly? LastVacationDate { get; private set; }


    public virtual ICollection<ShiftAssignment> Shifts { get; private set; } = new List<ShiftAssignment>();
    public virtual ICollection<Vacation> Vacations { get; private set; } = new List<Vacation>();


    protected Employee() { }


    public Employee(PersonName firstName, PersonName lastName, PhoneNumber phone, Manager manager)
        : this(Guid.NewGuid(), firstName, lastName, phone, manager, EmployeeStatus.Active, null) { }


    protected Employee(Guid id, PersonName firstName, PersonName lastName, PhoneNumber phone, Manager manager, EmployeeStatus status, DateOnly? lastVacationDate)
        : base(id)
    {
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        Phone = phone ?? throw new ArgumentNullException(nameof(phone));
        Manager = manager ?? throw new ArgumentNullException(nameof(manager));
        ManagerId = manager.Id;
        Status = status;
        LastVacationDate = lastVacationDate;
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

        
        if (start > end)
            throw new ArgumentException("Дата начала отпуска не может быть позже даты окончания.");

        
        if (LastVacationDate.HasValue)
        {
            if (start.DayNumber - LastVacationDate.Value.DayNumber < 180)
                throw new InvalidOperationException("Запрашивать отпуск можно не чаще 1 раза в 6 месяцев.");
        }

        
        bool hasActiveRequest = Vacations.Any(v => v.Status == VacationStatus.Requested);
        if (hasActiveRequest)
            throw new InvalidOperationException("У вас уже есть ожидающая рассмотрения заявка на отпуск.");

        
        var vacation = new Vacation(this, start, end);
        Vacations.Add(vacation);

        return vacation;
    }

    
    public void RegisterVacation(DateOnly startDate)
    {
        LastVacationDate = startDate;
    }
}