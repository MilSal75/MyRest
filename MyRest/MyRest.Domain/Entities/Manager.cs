#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using MyRest.Domain.ValueObjects;
using MyRest.Domain.Enums;

namespace MyRest.Domain.Entities
{
    public class Manager
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; }
        public Phone Phone { get; set; }
        public EmployeeStatus Status { get; set; }

        private List<Employee> _employees = new List<Employee>();
        public IReadOnlyCollection<Employee> Employees => _employees.AsReadOnly();

        public List<Employee> EmployeesForJson
        {
            get => _employees;
            set => _employees = value ?? new List<Employee>();
        }

        public Manager()
        {
        }

        public Manager(string fullName, Phone phone)
        {
            FullName = fullName;
            Phone = phone;
            Status = EmployeeStatus.Active;
        }

        public Employee CreateEmployee(string fullName, Phone phone)
        {
            var employee = new Employee(fullName, phone, this);
            _employees.Add(employee);
            return employee;
        }

        public void AssignShift(Employee employee, DateTime shiftDate, decimal hours)
        {
            if (!_employees.Contains(employee))
                throw new InvalidOperationException("Этот сотрудник не относится к данному менеджеру");
            employee.AssignShift(shiftDate, hours);
        }

        public void RemoveShift(Employee employee, DateTime shiftDate)
        {
            if (!_employees.Contains(employee))
                throw new InvalidOperationException("Этот сотрудник не относится к данному менеджеру");
            employee.RemoveShift(shiftDate);
        }

        public void MarkShiftDone(Employee employee, DateTime shiftDate)
        {
            if (!_employees.Contains(employee))
                throw new InvalidOperationException("Этот сотрудник не относится к данному менеджеру");
            employee.MarkShiftDone(shiftDate);
        }

        public void ApproveVacation(Employee employee)
        {
            if (!_employees.Contains(employee))
                throw new InvalidOperationException("Этот сотрудник не относится к данному менеджеру");
            employee.ApproveVacation();
        }

        public void RejectVacation(Employee employee)
        {
            if (!_employees.Contains(employee))
                throw new InvalidOperationException("Этот сотрудник не относится к данному менеджеру");
            employee.RejectVacation();
        }

        public IReadOnlyCollection<Employee> GetVacationRequests()
        {
            return _employees.Where(e => e.VacationRequested).ToList().AsReadOnly();
        }
    }
}