using System;
using System.Collections.Generic;
using System.Linq;
using MyRest.Domain.Enums;
using MyRest.Domain.Exceptions;
using MyRest.Domain.ValueObjects;

namespace MyRest.Domain.Entities;

public class Employee
{
    public Guid Id { get; } = Guid.NewGuid();
    public Guid ManagerId { get; }
    public PersonName FirstName { get; private set; }
    public PersonName LastName { get; private set; }
    public PhoneNumber Phone { get; private set; }
    public UserStatus Status { get; private set; } = UserStatus.Active;

    private readonly List<Vacation> _vacations = new();
    public IReadOnlyCollection<Vacation> Vacations => _vacations.AsReadOnly();

    private readonly List<ShiftAssignment> _assignments = new();
    public IReadOnlyCollection<ShiftAssignment> Assignments => _assignments.AsReadOnly();

    private Employee() { } // Пустой конструктор специально для EF Core

    internal Employee(Guid managerId, PersonName firstName, PersonName lastName, PhoneNumber phone)
    {
        ManagerId = managerId;
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
    }

    public Vacation RequestVacation(DateOnly start, DateOnly end)
    {
        // 1. Проверка статуса сотрудника
        if (Status == UserStatus.Fired)
            throw new InvalidEntityStateException("Уволенный сотрудник не может запрашивать отпуск.");

        // 2. Проверка правила "Раз в полгода"
        var lastVacation = _vacations
            .Where(v => v.Status != VacationStatus.Rejected)
            .OrderByDescending(v => v.EndDate)
            .FirstOrDefault();

        if (lastVacation != null)
        {
            if (lastVacation.EndDate.AddMonths(6) > start)
            {
                throw new InvalidEntityStateException(
                    $"Нарушение интервала: следующий отпуск возможен только после {lastVacation.EndDate.AddMonths(6):dd.MM.yyyy}.");
            }
        }

        // 3. Создание отпуска
        var vacation = new Vacation(this.Id, start, end);
        _vacations.Add(vacation);
        return vacation;
    }

    public void MarkAttendance(ShiftAssignment assignment)
    {
        if (assignment.EmployeeId != this.Id)
            throw new InvalidEntityStateException("Попытка отметить посещение чужой смены.");

        assignment.MarkCompleted();
    }

    internal void AddAssignment(ShiftAssignment assignment) => _assignments.Add(assignment);

    internal void Fire()
    {
        if (Status == UserStatus.Fired)
            throw new InvalidEntityStateException("Сотрудник уже имеет статус уволенного.");

        Status = UserStatus.Fired;
    }
}