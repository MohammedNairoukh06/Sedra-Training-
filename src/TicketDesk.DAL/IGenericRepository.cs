namespace TicketDesk.DAL;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllWithIncludesAsync(params string[] includeProperties);
    Task<T?> GetByIdWithIncludesAsync(int id, params string[] includeProperties);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task SaveChangesAsync();
}