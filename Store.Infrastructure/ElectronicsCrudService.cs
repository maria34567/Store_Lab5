using System.Collections;
using Store.Common;
using Store.Infrastructure.Models;
using Store.Infrastructure.Repositories;

namespace Store.Infrastructure;

public sealed class ElectronicsCrudService(IGuidRepository<ElectronicsModel> repository) : ICrudServiceAsync<Electronics>
{
    private static Electronics ToDomain(ElectronicsModel x) => new(x.Name, x.Price, x.Brand, x.WarrantyMonths) { Id = x.Id };
    private static ElectronicsModel ToModel(Electronics x) => new() { Id=x.Id, Name=x.Name, Price=x.Price, Brand=x.Brand, WarrantyMonths=x.WarrantyMonths, CategoryId=1 };
    public async Task<bool> CreateAsync(Electronics element)
    {
        if (await repository.GetByGuidAsync(element.Id) is not null) return false;
        await repository.AddAsync(ToModel(element)); return true;
    }
    public async Task<Electronics> ReadAsync(Guid id) => ToDomain(await repository.GetByGuidAsync(id) ?? throw new KeyNotFoundException($"Product {id} not found"));
    public async Task<IEnumerable<Electronics>> ReadAllAsync() => (await repository.GetAllAsync()).Select(ToDomain).ToArray();
    public async Task<IEnumerable<Electronics>> ReadAllAsync(int page, int amount)
    {
        if(page < 1 || amount < 1) throw new ArgumentOutOfRangeException(nameof(page), "Page and amount must be positive");
        return (await repository.GetPageAsync(page,amount)).Select(ToDomain).ToArray();
    }
    public async Task<bool> UpdateAsync(Electronics element)
    {
        if (await repository.GetByGuidAsync(element.Id) is null) return false;
        await repository.Update(ToModel(element)); return true;
    }
    public async Task<bool> RemoveAsync(Electronics element)
    {
        var model = await repository.GetByGuidAsync(element.Id);
        if(model is null) return false;
        await repository.Delete(model); return true;
    }
    public Task<bool> SaveAsync() => Task.FromResult(true); // Repository persists each mutation with SaveChangesAsync.
    public IEnumerator<Electronics> GetEnumerator() => ReadAllAsync().GetAwaiter().GetResult().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
