using MyRest.Domain.Entities;
using MyRest.Domain.Repositories;

namespace MyRest.Infrastructure.RepositoriesEF;

public class ManagerRepository : Repository<Manager>, IManagerRepository
{
    public ManagerRepository(ApplicationDbContext context) : base(context)
    {
    }
}