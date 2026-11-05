using Microsoft.EntityFrameworkCore;
namespace Store.Infrastructure.Repositories;

public class EfRepository<T>(StoreContext context) : IRepository<T> where T : class
{
    public async Task<T?> GetByIdAsync(int id) => await context.Set<T>().FindAsync(id);
    public async Task<IEnumerable<T>> GetAllAsync() => await context.Set<T>().AsNoTracking().ToListAsync();
    public async Task AddAsync(T entity) { await context.Set<T>().AddAsync(entity); await context.SaveChangesAsync(); }
    public async Task Update(T entity) { context.Set<T>().Update(entity); await context.SaveChangesAsync(); }
    public async Task Delete(T entity) { context.Set<T>().Remove(entity); await context.SaveChangesAsync(); }
}
