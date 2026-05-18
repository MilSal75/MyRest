using MyRest.Domain.Entities;
using MyRest.Domain.Repositories.Abstractions;

namespace MyRest.Infrastructure.EntityFramework.RepositoriesEF;

public class EmployeeRepository : EFRepository<Employee, Guid>, IEmployeeRepository
{
    public EmployeeRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}