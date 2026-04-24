using MyRest.Domain.Enums;
using System;

namespace MyRest.Domain.Entities;

public class Vacation
{
    public Guid Id { get; } = Guid.NewGuid();
    public Guid EmployeeId { get; private set; }
    public Employee Employee { get; private set; }

    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public VacationStatus Status { get; private set; } = VacationStatus.Requested;

    private Vacation() { }

    public Vacation(Employee employee, DateTime start, DateTime end)
    {
        Employee = employee ?? throw new ArgumentNullException(nameof(employee));
        EmployeeId = employee.Id;

        if (end < start)
            throw new ArgumentOutOfRangeException(nameof(end), "Дата окончания отпуска не может быть раньше даты начала");

        StartDate = start;
        EndDate = end;
    }

    public void Approve() => Status = VacationStatus.Approved;
    public void Reject() => Status = VacationStatus.Rejected;
}