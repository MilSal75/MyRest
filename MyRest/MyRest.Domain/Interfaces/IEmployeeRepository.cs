using MyRest.Domain.Entities;

namespace MyRest.Domain.Interfaces;

public interface IEmployeeRepository
{
    Employee GetById(Guid id);
    IEnumerable<Employee> GetByManagerId(Guid managerId);
    IEnumerable<Employee> GetWithVacationRequests();
    void Add(Employee employee);
    void Update(Employee employee);
    void Delete(Guid id);
}