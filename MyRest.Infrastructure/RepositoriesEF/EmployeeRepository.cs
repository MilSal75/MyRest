using Microsoft.EntityFrameworkCore;
using MyRest.Domain.Entities;
using MyRest.Domain.Repositories;

namespace MyRest.Infrastructure.RepositoriesEF;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Employee?> GetByIdAsync(Guid id)
    {
        // Подгружаем вместе с сотрудником его отпуска и смены
        return await _dbSet
            .Include(e => e.Vacations)
            .Include(e => e.Assignments)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public override async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _dbSet
            .Include(e => e.Vacations)
            .Include(e => e.Assignments)
            .ToListAsync();
    }
}