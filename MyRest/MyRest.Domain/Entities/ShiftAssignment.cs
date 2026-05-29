using System;
using MyRest.Domain.Base;
using MyRest.Domain.Enums;
using MyRest.Domain.Exceptions;

namespace MyRest.Domain.Entities;


public class ShiftAssignment : Entity<Guid>
{
    public Employee Employee { get; }
    public Shift Shift { get; }
    public AssignmentStatus Status { get; private set; }

    
    protected ShiftAssignment() { }

    public ShiftAssignment(Employee employee, Shift shift)
        : this(Guid.NewGuid(), employee, shift, AssignmentStatus.Planned) { }

    protected ShiftAssignment(Guid id, Employee employee, Shift shift, AssignmentStatus status)
        : base(id)
    {
        Employee = employee ?? throw new ArgumentNullException(nameof(employee));
        Shift = shift ?? throw new ArgumentNullException(nameof(shift));
        Status = status;
    }


    public void Complete(Guid userId)
    {
        if (Status != AssignmentStatus.Planned)
            throw new InvalidOperationException("Можно завершить только назначенную смену.");

        
        if (userId != Employee.Id && userId != Employee.ManagerId)
        {
            
            throw new InvalidOperationException("Завершить смену может только сам сотрудник или его менеджер.");
        }

        Status = AssignmentStatus.Completed;
    }


    public void Cancel(Manager manager)
    {
        if (manager == null)
            throw new ArgumentNullException(nameof(manager));

        if (Status != AssignmentStatus.Planned)
            throw new InvalidOperationException("Можно отменить только назначенную смену.");

        if (manager.Id != Employee.ManagerId)
            throw new EmployeeNotBelongManagerException(manager, Employee);

        Status = AssignmentStatus.Cancelled;
    }
}