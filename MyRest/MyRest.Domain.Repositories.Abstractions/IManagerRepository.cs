using MyRest.Domain.Entities;
using MyRest.Domain.Repositories.Abstractions.Base;

namespace MyRest.Domain.Repositories.Abstractions;

public interface IManagerRepository : IRepository<Manager, Guid>
{
}