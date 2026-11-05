using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Models;
namespace Store.Infrastructure.Repositories;

// Domain products use Guid IDs; IRepository<T> requires int IDs, so this extension supports the domain key.
public interface IGuidRepository<T> : IRepository<T> where T : ProductModel
{
    Task<T?> GetByGuidAsync(Guid id);
    Task<IEnumerable<T>> GetPageAsync(int page, int amount);
}
public class GuidRepository<T>(StoreContext context) : IGuidRepository<T> where T : ProductModel
{
    public Task<T?> GetByIdAsync(int id) => throw new NotSupportedException("Products use Guid IDs. Use GetByGuidAsync.");
    public async Task<T?> GetByGuidAsync(Guid id) => await context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    public async Task<IEnumerable<T>> GetAllAsync() => await context.Set<T>().AsNoTracking().OrderBy(x => x.Id).ToListAsync();
    public async Task<IEnumerable<T>> GetPageAsync(int page, int amount) => await context.Set<T>().AsNoTracking().OrderBy(x => x.Id).Skip((page-1)*amount).Take(amount).ToListAsync();
    public async Task AddAsync(T entity) { await context.Set<T>().AddAsync(entity); await context.SaveChangesAsync(); }
    public async Task Update(T entity)
    {
        var entry = context.Entry(entity);
        var key = entity.Id;
        var tracked = context.Set<T>().Local.FirstOrDefault(x => x.Id == key);
        if (tracked is not null && !ReferenceEquals(tracked, entity))
        {
            context.Entry(tracked).CurrentValues.SetValues(entity);
        }
        else
        {
            context.Set<T>().Update(entity);
        }
        await context.SaveChangesAsync();
    }
    public async Task Delete(T entity)
    {
        var tracked = context.Set<T>().Local.FirstOrDefault(x => x.Id == entity.Id);
        context.Set<T>().Remove(tracked ?? entity);
        await context.SaveChangesAsync();
    }
}
