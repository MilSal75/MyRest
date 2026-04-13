using MyRest.Domain.Enums;
using MyRest.Domain.Exceptions;

namespace MyRest.Domain.Entities;

public class ShiftAssignment
{
    public Guid Id { get; } = Guid.NewGuid();
    public Guid EmployeeId { get; }
    public Guid ShiftId { get; }
    public AssignmentStatus Status { get; private set; } = AssignmentStatus.Planned;

    internal ShiftAssignment(Guid employeeId, Guid shiftId)
    {
        EmployeeId = employeeId;
        ShiftId = shiftId;
    }

    internal void MarkCompleted()
    {
        if (Status == AssignmentStatus.Cancelled)
            throw new InvalidEntityStateException("Нельзя завершить отмененную смену.");

        Status = AssignmentStatus.Completed;
    }
}