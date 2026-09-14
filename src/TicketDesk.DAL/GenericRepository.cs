using Microsoft.EntityFrameworkCore;

namespace TicketDesk.DAL;

public class GenericRepository<T>(TicketDeskDbContext context) : IGenericRepository<T> where T : class
{
    private readonly TicketDeskDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

    public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

    public async Task<IEnumerable<T>> GetAllWithIncludesAsync(params string[] includeProperties)
    {
        IQueryable<T> query = _dbSet;

        foreach (var property in includeProperties)
        {
            if (!string.IsNullOrWhiteSpace(property))
            {
                query = query.Include(property);
            }
        }

        return await query.ToListAsync();
    }

    public async Task<T?> GetByIdWithIncludesAsync(int id, params string[] includeProperties)
    {
        IQueryable<T> query = _dbSet;

        foreach (var property in includeProperties)
        {
            if (!string.IsNullOrWhiteSpace(property))
            {
                query = query.Include(property);
            }
        }

        return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
    }

    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

    public void Update(T entity) => _dbSet.Update(entity);

    public void Delete(T entity) => _dbSet.Remove(entity);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}