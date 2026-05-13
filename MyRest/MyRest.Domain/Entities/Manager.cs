using MyRest.Domain.Base;
using MyRest.Domain.Exceptions;
using MyRest.Domain.ValueObjects;

namespace MyRest.Domain.Entities;

public class Manager(Guid id, PersonName firstName, PersonName lastName) : Entity<Guid>(id)
{
    private readonly ICollection<Employee> _employees = [];

    public PersonName FirstName { get; private set; } = firstName ?? throw new ArgumentNullException(nameof(firstName));
    public PersonName LastName { get; private set; } = lastName ?? throw new ArgumentNullException(nameof(lastName));
    public IReadOnlyCollection<Employee> Employees => _employees.ToList().AsReadOnly();

    protected Manager() : this(Guid.NewGuid(), default!, default!) { }

    public Employee HireEmployee(PersonName firstName, PersonName lastName, PhoneNumber phone)
    {
        var employee = new Employee(this, firstName, lastName, phone);
        _employees.Add(employee);
        return employee;
    }

    public bool FireEmployee(Employee employee)
    {
        if (employee.Manager.Id != this.Id) throw new EmployeeNotBelongManagerException(this, employee);
        return employee.Fire();
    }
}