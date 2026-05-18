using MyRest.Domain.Entities;
using MyRest.Domain.Repositories.Abstractions;

namespace MyRest.Infrastructure.EntityFramework.RepositoriesEF;

public class ManagerRepository : EFRepository<Manager, Guid>, IManagerRepository
{
    public ManagerRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}