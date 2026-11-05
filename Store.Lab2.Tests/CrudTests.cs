using Store.Common;
using Xunit;
namespace Store.Lab2.Tests;
public class CrudTests
{
    private static AsyncCrudService<Electronics> NewService() => new(Path.Combine(Path.GetTempPath(),Guid.NewGuid()+".json"));
    [Fact] public async Task CreateReadUpdateRemove()
    {
        var s=NewService(); var x=Electronics.CreateNew();
        Assert.True(await s.CreateAsync(x));
        Assert.False(await s.CreateAsync(x));
        Assert.Equal(x.Id,(await s.ReadAsync(x.Id)).Id);
        var updated=new Electronics("Test",123m,"A",12){Id=x.Id};
        Assert.True(await s.UpdateAsync(updated));
        Assert.Equal(123m,(await s.ReadAsync(x.Id)).Price);
        Assert.True(await s.RemoveAsync(updated));
        Assert.False(await s.RemoveAsync(updated));
        await Assert.ThrowsAsync<KeyNotFoundException>(()=>s.ReadAsync(x.Id));
    }
    [Fact] public async Task PaginationAndEnumerable()
    {
        var s=NewService();
        for(int i=0;i<12;i++) await s.CreateAsync(Electronics.CreateNew());
        Assert.Equal(12,s.Count());
        Assert.Equal(5,(await s.ReadAllAsync(1,5)).Count());
        Assert.Equal(2,(await s.ReadAllAsync(3,5)).Count());
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(()=>s.ReadAllAsync(0,5));
    }
    [Fact] public async Task ParallelCreateAndSave()
    {
        var s=NewService();
        Parallel.For(0,1000,_=>s.CreateAsync(Electronics.CreateNew()).GetAwaiter().GetResult());
        Assert.Equal(1000,(await s.ReadAllAsync()).Count());
        Assert.True(await s.SaveAsync());
        Assert.True(File.Exists(s.FilePath));
        File.Delete(s.FilePath);
    }
}
