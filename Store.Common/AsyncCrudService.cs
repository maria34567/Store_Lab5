using System.Collections;
using System.Text.Json;
namespace Store.Common;
public sealed class AsyncCrudService<T> : ICrudServiceAsync<T> where T : Product
{
    private readonly Dictionary<Guid,T> _items = new();
    private readonly object _gate = new();
    private readonly SemaphoreSlim _saveGate = new(1,1);
    public string FilePath { get; }
    public AsyncCrudService(string filePath) => FilePath = filePath;
    public Task<bool> CreateAsync(T element)
    {
        ArgumentNullException.ThrowIfNull(element);
        lock (_gate) return Task.FromResult(_items.TryAdd(element.Id,element));
    }
    public Task<T> ReadAsync(Guid id)
    {
        lock (_gate)
            return Task.FromResult(_items.TryGetValue(id,out var item) ? item : throw new KeyNotFoundException($"ID {id} не знайдено"));
    }
    public Task<IEnumerable<T>> ReadAllAsync()
    {
        lock (_gate) return Task.FromResult<IEnumerable<T>>(_items.Values.ToArray());
    }
    public Task<IEnumerable<T>> ReadAllAsync(int page,int amount)
    {
        if (page < 1 || amount < 1) throw new ArgumentOutOfRangeException(nameof(page),"Номер сторінки і розмір мають бути додатними.");
        lock (_gate) return Task.FromResult<IEnumerable<T>>(_items.Values.OrderBy(x=>x.Id).Skip(((long)page-1)*amount > int.MaxValue ? int.MaxValue : (int)(((long)page-1)*amount)).Take(amount).ToArray());
    }
    public Task<bool> UpdateAsync(T element)
    {
        ArgumentNullException.ThrowIfNull(element);
        lock (_gate)
        {
            if (!_items.ContainsKey(element.Id)) return Task.FromResult(false);
            _items[element.Id]=element;
            return Task.FromResult(true);
        }
    }
    public Task<bool> RemoveAsync(T element)
    {
        ArgumentNullException.ThrowIfNull(element);
        lock (_gate) return Task.FromResult(_items.Remove(element.Id));
    }
    public async Task<bool> SaveAsync()
    {
        await _saveGate.WaitAsync();
        try
        {
            T[] snapshot;
            lock (_gate) snapshot = _items.Values.ToArray();
            var directory=Path.GetDirectoryName(Path.GetFullPath(FilePath));
            if (directory is not null) Directory.CreateDirectory(directory);
            string temp=FilePath+"."+Guid.NewGuid().ToString("N")+".tmp";
            try
            {
                await using(var stream=new FileStream(temp,FileMode.CreateNew,FileAccess.Write,FileShare.None,81920,useAsync:true))
                    await JsonSerializer.SerializeAsync(stream,snapshot,new JsonSerializerOptions{WriteIndented=true});
                File.Move(temp,FilePath,true);
                return true;
            }
            finally { if(File.Exists(temp)) File.Delete(temp); }
        }
        finally { _saveGate.Release(); }
    }
    public IEnumerator<T> GetEnumerator()
    {
        lock (_gate) return ((IEnumerable<T>)_items.Values.ToArray()).GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()=>GetEnumerator();
}
