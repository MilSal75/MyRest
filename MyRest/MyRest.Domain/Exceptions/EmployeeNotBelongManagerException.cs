using MyRest.Domain.Entities;

namespace MyRest.Domain.Exceptions;

public class EmployeeNotBelongManagerException(Manager manager, Employee employee)
    : Exception($"Сотрудник {employee.Id} не принадлежит менеджеру {manager.Id}");