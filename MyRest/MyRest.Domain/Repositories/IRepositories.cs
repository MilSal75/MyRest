using MyRest.Domain.Entities;

namespace MyRest.Domain.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}

public interface IEmployeeRepository : IRepository<Employee> { }
public interface IManagerRepository : IRepository<Manager> { }
public interface IShiftRepository : IRepository<Shift> { }