using System;
using MyRest.Domain.Base;
using MyRest.Domain.Enums;
using MyRest.Domain.Exceptions;

namespace MyRest.Domain.Entities;


public class Vacation : Entity<Guid>
{
    public Employee Employee { get; }
    public DateOnly StartDate { get; }
    public DateOnly EndDate { get; }
    public VacationStatus Status { get; private set; }

    
    protected Vacation() { }

    public Vacation(Employee employee, DateOnly startDate, DateOnly endDate)
        : this(Guid.NewGuid(), employee, startDate, endDate, VacationStatus.Requested) { }

    protected Vacation(Guid id, Employee employee, DateOnly startDate, DateOnly endDate, VacationStatus status)
        : base(id)
    {
        if (startDate > endDate)
            throw new ArgumentException("Дата начала не может быть позже окончания.");

        if (endDate.DayNumber - startDate.DayNumber > 28)
            throw new ArgumentException("Отпуск не может длиться больше 28 дней.");

        Employee = employee ?? throw new ArgumentNullException(nameof(employee));
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
    }

    
    public void Approve(Manager manager)
    {
        if (manager == null)
            throw new ArgumentNullException(nameof(manager));

        if (manager.Id != Employee.ManagerId)
            throw new EmployeeNotBelongManagerException(manager, Employee);

        if (Status != VacationStatus.Requested)
            throw new InvalidOperationException("Можно одобрить только заявку в статусе Requested.");

        Status = VacationStatus.Approved;
    }

    public void Reject(Manager manager)
    {
        if (manager == null)
            throw new ArgumentNullException(nameof(manager));

        if (manager.Id != Employee.ManagerId)
            throw new EmployeeNotBelongManagerException(manager, Employee);

        if (Status != VacationStatus.Requested)
            throw new InvalidOperationException("Можно отклонить только заявку в статусе Requested.");

        Status = VacationStatus.Rejected;
    }
}