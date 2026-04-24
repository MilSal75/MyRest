using MyRest.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace MyRest.Domain.Entities;

public class Manager
{
    public Guid Id { get; } = Guid.NewGuid();
    public PersonName FirstName { get; private set; }
    public PersonName LastName { get; private set; }

    private List<Employee> _employees = new List<Employee>();
    public IReadOnlyCollection<Employee> Employees => _employees.AsReadOnly();

    // Пустой конструктор для EF Core
    private Manager() { }

    public Manager(string firstName, string lastName)
    {
        FirstName = new PersonName(firstName);
        LastName = new PersonName(lastName);
    }

    public Employee HireEmployee(string fName, string lName, string phone)
    {
        var employee = new Employee(this, fName, lName, phone);
        _employees.Add(employee);
        return employee;
    }

    public void FireEmployee(Employee employee)
    {
        if (!_employees.Contains(employee))
            throw new InvalidOperationException("Попытка уволить сотрудника другого менеджера");

        employee.Fire();
    }
}