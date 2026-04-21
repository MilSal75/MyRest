using System;
using MyRest.Domain.Enums;
using MyRest.Domain.Exceptions;

namespace MyRest.Domain.Entities;

public class Vacation
{
    public Guid Id { get; } = Guid.NewGuid();
    public Guid EmployeeId { get; }
    public DateOnly StartDate { get; }
    public DateOnly EndDate { get; }
    public VacationStatus Status { get; private set; } = VacationStatus.Requested;

    private Vacation() { } // Пустой конструктор специально для EF Core
    internal Vacation(Guid employeeId, DateOnly startDate, DateOnly endDate)
    {
        if (startDate >= endDate)
            throw new InvalidEntityStateException("Дата окончания отпуска должна быть позже даты начала.");

        EmployeeId = employeeId;
        StartDate = startDate;
        EndDate = endDate;
    }

    internal void Approve() => Status = VacationStatus.Approved;
    internal void Reject() => Status = VacationStatus.Rejected;

    // Тот самый новый метод для вывода статуса с датой
    public string GetStatusInfo()
    {
        if (Status == VacationStatus.Approved)
        {
            return $"Approved (до {EndDate:dd.MM.yyyy})";
        }
        return Status.ToString();
    }
}