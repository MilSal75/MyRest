using MyRest.Domain.Base;
using MyRest.Domain.Enums;

namespace MyRest.Domain.Entities;

public class ShiftAssignment : Entity<Guid>
{
    public Employee Employee { get; private set; }
    public Shift Shift { get; private set; }
    public AssignmentStatus Status { get; private set; } = AssignmentStatus.Planned;

    private ShiftAssignment() : base(Guid.NewGuid()) { }

    public ShiftAssignment(Guid id, Employee employee, Shift shift)
        : base(id)
    {
        Employee = employee ?? throw new ArgumentNullException(nameof(employee));
        Shift = shift ?? throw new ArgumentNullException(nameof(shift));
    }

    public void Complete(Guid userId)
    {
        if (Status != AssignmentStatus.Planned)
            throw new InvalidOperationException("Можно завершить только назначенную смену.");

        if (userId != Employee.Id && userId != Employee.ManagerId)
            throw new UnauthorizedAccessException("Завершить смену может только сам сотрудник или его менеджер.");

        Status = AssignmentStatus.Completed;
    }

    public void Cancel(Manager manager)
    {
        if (manager == null)
            throw new ArgumentNullException(nameof(manager));

        if (Status != AssignmentStatus.Planned)
            throw new InvalidOperationException("Можно отменить только назначенную смену.");

        if (manager.Id != Employee.ManagerId)
            throw new UnauthorizedAccessException("Отменить смену может только менеджер этого сотрудника.");

        Status = AssignmentStatus.Cancelled;
    }
}