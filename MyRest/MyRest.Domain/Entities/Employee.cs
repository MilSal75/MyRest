#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using MyRest.Domain.ValueObjects;
using MyRest.Domain.Enums;

namespace MyRest.Domain.Entities
{
    public class Employee
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; }
        public Phone Phone { get; set; }
        public EmployeeStatus Status { get; set; }
        public Manager Manager { get; set; }

        public bool VacationRequested { get; set; } = false;
        public VacationStatus VacationStatus { get; set; } = VacationStatus.Pending;

        private List<Shift> _shifts = new List<Shift>();
        public IReadOnlyCollection<Shift> Shifts => _shifts.AsReadOnly();

        public List<Shift> ShiftsForJson
        {
            get => _shifts;
            set => _shifts = value ?? new List<Shift>();
        }

        public Employee()
        {
        }

        public Employee(string fullName, Phone phone, Manager manager)
        {
            FullName = fullName;
            Phone = phone;
            Manager = manager;
            Status = EmployeeStatus.Active;
        }

        public void AssignShift(DateTime shiftDate, decimal hours)
        {
            if (hours <= 0 || hours > 24)
                throw new ArgumentOutOfRangeException(nameof(hours), "Часы смены должны быть от 1 до 24");

            if (shiftDate < DateTime.Today)
                throw new InvalidOperationException("Нельзя назначить смену на прошедшую дату");

            _shifts.Add(new Shift(shiftDate, hours));
        }

        public void RemoveShift(DateTime shiftDate)
        {
            var shift = _shifts.FirstOrDefault(s => s.ShiftDate == shiftDate);
            if (shift != null)
                _shifts.Remove(shift);
        }

        public void MarkShiftDone(DateTime shiftDate)
        {
            var shift = _shifts.FirstOrDefault(s => s.ShiftDate == shiftDate);
            if (shift == null)
                throw new InvalidOperationException("Нет смены на эту дату");
            shift.MarkDone();
        }

        public void RequestVacation()
        {
            if (Status != EmployeeStatus.Active)
                throw new InvalidOperationException("Нельзя запросить отпуск в текущем статусе");

            VacationRequested = true;
            VacationStatus = VacationStatus.Pending;
        }

        public void ApproveVacation()
        {
            if (!VacationRequested)
                throw new InvalidOperationException("Нет запроса на отпуск");

            VacationStatus = VacationStatus.Approved;
            Status = EmployeeStatus.Vacation;
            VacationRequested = false;
        }

        public void RejectVacation()
        {
            if (!VacationRequested)
                throw new InvalidOperationException("Нет запроса на отпуск");

            VacationStatus = VacationStatus.Rejected;
            VacationRequested = false;
        }

        public override string ToString()
        {
            string shortId = Id.ToString().Substring(0, 8);
            string shiftsInfo = _shifts.Count == 0 ? "нет смен" : string.Join(", ", _shifts.Select(s => s.ToString()));
            string statusText = Status switch
            {
                EmployeeStatus.Active => "работает",
                EmployeeStatus.Vacation => "в отпуске",
                EmployeeStatus.Fired => "уволен",
                _ => Status.ToString()
            };
            return $"[{shortId}] {FullName} | {Phone.Value} | {statusText} | Смены: {shiftsInfo}";
        }
    }
}