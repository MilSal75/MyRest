using MyRest.Domain.Entities;
using MyRest.Domain.Repositories.Abstractions.Base;

namespace MyRest.Domain.Repositories.Abstractions;

public interface IEmployeeRepository : IRepository<Employee, Guid>
{
}