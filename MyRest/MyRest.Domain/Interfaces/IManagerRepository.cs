using MyRest.Domain.Entities;

namespace MyRest.Domain.Interfaces;

public interface IManagerRepository
{
    Manager GetById(Guid id);
    IEnumerable<Manager> GetAll();
    void Add(Manager manager);
    void Update(Manager manager);
    void Delete(Guid id);
}