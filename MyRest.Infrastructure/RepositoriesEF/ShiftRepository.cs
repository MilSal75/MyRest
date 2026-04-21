using Microsoft.EntityFrameworkCore;
using MyRest.Domain.Entities;
using MyRest.Domain.Repositories;

namespace MyRest.Infrastructure.RepositoriesEF;

public class ShiftRepository : Repository<Shift>, IShiftRepository
{
    public ShiftRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Shift?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(s => s.Assignments)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public override async Task<IEnumerable<Shift>> GetAllAsync()
    {
        return await _dbSet
            .Include(s => s.Assignments)
            .ToListAsync();
    }
}