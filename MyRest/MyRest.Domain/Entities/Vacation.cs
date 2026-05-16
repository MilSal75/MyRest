using MyRest.Domain.Base;
using MyRest.Domain.Enums;

namespace MyRest.Domain.Entities;

public class Vacation : Entity<Guid>
{
    public Employee Employee { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public VacationStatus Status { get; private set; } = VacationStatus.Requested;

    private Vacation() : base(Guid.NewGuid()) { }

    public Vacation(Guid id, Employee employee, DateOnly startDate, DateOnly endDate)
        : base(id)
    {
        if (startDate > endDate)
            throw new ArgumentException("Дата начала отпуска не может быть позже даты окончания.");

        Employee = employee ?? throw new ArgumentNullException(nameof(employee));
        StartDate = startDate;
        EndDate = endDate;
    }

    public void Approve(Manager manager)
    {
        if (manager == null)
            throw new ArgumentNullException(nameof(manager));

        if (manager.Id != Employee.ManagerId)
            throw new UnauthorizedAccessException("Одобрить отпуск может только менеджер этого сотрудника.");

        if (Status != VacationStatus.Requested)
            throw new InvalidOperationException("Можно одобрить только заявку в статусе Requested.");

        Status = VacationStatus.Approved;
    }

    public void Reject(Manager manager)
    {
        if (manager == null)
            throw new ArgumentNullException(nameof(manager));

        if (manager.Id != Employee.ManagerId)
            throw new UnauthorizedAccessException("Отклонить отпуск может только менеджер этого сотрудника.");

        if (Status != VacationStatus.Requested)
            throw new InvalidOperationException("Можно отклонить только заявку в статусе Requested.");

        Status = VacationStatus.Rejected;
    }
}