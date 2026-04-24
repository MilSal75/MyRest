using System;
using System.Collections.Generic;

namespace MyRest.Domain.Entities;

public class Shift
{
    public Guid Id { get; } = Guid.NewGuid();
    public Guid ManagerId { get; private set; }

    public DateTime ShiftDate { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public decimal DurationHours { get; private set; }

    private List<ShiftAssignment> _assignments = new List<ShiftAssignment>();
    public IReadOnlyCollection<ShiftAssignment> Assignments => _assignments.AsReadOnly();

    private Shift() { }

    public Shift(Manager manager, DateTime date, TimeSpan startTime, decimal duration)
    {
        if (manager == null)
            throw new ArgumentNullException(nameof(manager));

        if (duration <= 0 || duration > 12)
            throw new ArgumentOutOfRangeException(nameof(duration), "Длительность смены должна быть от 1 до 12 часов");

        ManagerId = manager.Id;
        ShiftDate = date;
        StartTime = startTime;
        DurationHours = duration;
    }

    public ShiftAssignment AssignEmployee(Employee employee)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));

        var assignment = new ShiftAssignment(employee, this);
        _assignments.Add(assignment);

        return assignment;
    }
}