using MyRest.Domain.Base;
using MyRest.Domain.ValueObjects;
using MyRest.Domain.Exceptions;

namespace MyRest.Domain.Entities;

public class Manager : Entity<Guid>
{
    public PersonName FirstName { get; private set; } = default!;
    public PersonName LastName { get; private set; } = default!;
    public PhoneNumber Phone { get; private set; } = default!;

    private readonly List<Employee> _employees = new();
    public IReadOnlyCollection<Employee> Employees => _employees.AsReadOnly();

    private Manager() : base(Guid.NewGuid()) { }

    public Manager(Guid id, PersonName firstName, PersonName lastName, PhoneNumber phone)
        : base(id)
    {
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        Phone = phone ?? throw new ArgumentNullException(nameof(phone));
    }

    public Employee HireEmployee(PersonName firstName, PersonName lastName, PhoneNumber phone)
    {
        var employee = new Employee(Guid.NewGuid(), firstName, lastName, phone, this);
        _employees.Add(employee);
        return employee;
    }

    public bool FireEmployee(Employee employee)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));

        return employee.Fire(this);
    }

    public void ResolveVacation(Employee employee, Vacation vacation, bool approve)
    {
        if (employee == null) throw new ArgumentNullException(nameof(employee));
        if (vacation == null) throw new ArgumentNullException(nameof(vacation));

        if (employee.ManagerId != Id)
            throw new EmployeeNotBelongManagerException(this, employee);

        if (approve)
            vacation.Approve(this);
        else
            vacation.Reject(this);
    }
}