using MyRest.Domain.Enums;
using System;

namespace MyRest.Domain.Entities;

public class ShiftAssignment
{
    public Guid Id { get; } = Guid.NewGuid();
    public Guid EmployeeId { get; private set; }
    public Guid ShiftId { get; private set; }

    public Employee Employee { get; private set; }
    public Shift Shift { get; private set; }

    public AssignmentStatus Status { get; private set; } = AssignmentStatus.Planned;

    private ShiftAssignment() { }

    public ShiftAssignment(Employee employee, Shift shift)
    {
        Employee = employee ?? throw new ArgumentNullException(nameof(employee));
        Shift = shift ?? throw new ArgumentNullException(nameof(shift));

        EmployeeId = employee.Id;
        ShiftId = shift.Id;
    }

    public void Complete() => Status = AssignmentStatus.Completed;
    public void Cancel() => Status = AssignmentStatus.Cancelled;
}