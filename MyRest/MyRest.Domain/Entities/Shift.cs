using MyRest.Domain.Exceptions;

namespace MyRest.Domain.Entities;

public class Shift
{
    public Guid Id { get; } = Guid.NewGuid();
    public Guid ManagerId { get; }
    public DateOnly ShiftDate { get; }
    public TimeOnly StartTime { get; }
    public decimal DurationHours { get; }

    private readonly List<ShiftAssignment> _assignments = new();
    public IReadOnlyCollection<ShiftAssignment> Assignments => _assignments.AsReadOnly();

    internal Shift(Guid managerId, DateOnly shiftDate, TimeOnly startTime, decimal durationHours)
    {
        if (durationHours <= 0 || durationHours > 12)
            throw new InvalidEntityStateException("Длительность смены должна быть от 1 до 12 часов.");

        ManagerId = managerId;
        ShiftDate = shiftDate;
        StartTime = startTime;
        DurationHours = durationHours;
    }

    internal void AddAssignment(ShiftAssignment assignment)
    {
        if (!_assignments.Any(a => a.EmployeeId == assignment.EmployeeId))
            _assignments.Add(assignment);
    }
}