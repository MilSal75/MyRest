using MyRest.Domain.Entities;

namespace MyRest.Domain.Exceptions;

public class EmployeeNotBelongManagerException(Manager manager, Employee employee)
    : Exception($"Сотрудник {employee.FirstName.Value} не принадлежит менеджеру {manager.FirstName.Value}");