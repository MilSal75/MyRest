using System;
using System.Collections.Generic;
using MyRest.Domain.Base;
using MyRest.Domain.ValueObjects;
using MyRest.Domain.Exceptions;

namespace MyRest.Domain.Entities;

public class Manager : Entity<Guid>
{
    public PersonName FirstName { get; private set; }
    public PersonName LastName { get; private set; }
    public PhoneNumber Phone { get; private set; }

    
    public virtual ICollection<Employee> Employees { get; private set; } = new List<Employee>();

    
    protected Manager() { }

    
    public Manager(PersonName firstName, PersonName lastName, PhoneNumber phone)
        : this(Guid.NewGuid(), firstName, lastName, phone) { }

    
    protected Manager(Guid id, PersonName firstName, PersonName lastName, PhoneNumber phone)
        : base(id)
    {
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        Phone = phone ?? throw new ArgumentNullException(nameof(phone));
    }

    public Employee HireEmployee(PersonName firstName, PersonName lastName, PhoneNumber phone)
    {
        
        var employee = new Employee(firstName, lastName, phone, this);
        Employees.Add(employee);
        return employee;
    }

    public bool FireEmployee(Employee employee)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));

        return employee.Fire(this);
    }



    public void ApproveVacation(Employee employee, Vacation vacation)
    {
        if (employee == null) throw new ArgumentNullException(nameof(employee));
        if (vacation == null) throw new ArgumentNullException(nameof(vacation));

        if (employee.ManagerId != Id)
            throw new EmployeeNotBelongManagerException(this, employee);

        
        if (employee.LastVacationDate.HasValue)
        {
            var daysSinceLastVacation = vacation.StartDate.DayNumber - employee.LastVacationDate.Value.DayNumber;
            if (daysSinceLastVacation < 180)
            {
                throw new InvalidOperationException("Сотрудник может брать отпуск не чаще 1 раза в 6 месяцев.");
            }
        }

        vacation.Approve(this);
        employee.RegisterVacation(vacation.StartDate);
    }

    public void RejectVacation(Employee employee, Vacation vacation)
    {
        if (employee == null) throw new ArgumentNullException(nameof(employee));
        if (vacation == null) throw new ArgumentNullException(nameof(vacation));

        if (employee.ManagerId != Id)
            throw new EmployeeNotBelongManagerException(this, employee);

        vacation.Reject(this);
    }
}