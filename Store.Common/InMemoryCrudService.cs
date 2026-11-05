using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Store.Common
{
    public delegate void ItemAddedHandler<T>(T item);

    public class InMemoryCrudService<T> : ICrudService<T>
    {
        private readonly List<T> _items = new List<T>();
        private readonly System.Reflection.PropertyInfo? _idProperty;

        public event ItemAddedHandler<T>? OnItemAdded;

        public InMemoryCrudService()
        {
            _idProperty = typeof(T).GetProperty("Id");
        }

        public void Create(T element)
        {
            _items.Add(element);
            OnItemAdded?.Invoke(element);
        }

        public T Read(Guid id)
        {
            if (_idProperty == null) 
                throw new InvalidOperationException("Тип не містить властивості Id.");

            var item = _items.FirstOrDefault(x => (Guid)(_idProperty.GetValue(x) ?? Guid.Empty) == id);
            return item ?? throw new KeyNotFoundException("Елемент не знайдено.");
        }

        public IEnumerable<T> ReadAll() => _items;

        public void Update(T element)
        {
            if (_idProperty == null) return;
            var id = (Guid)(_idProperty.GetValue(element) ?? Guid.Empty);
            var index = _items.FindIndex(x => (Guid)(_idProperty.GetValue(x) ?? Guid.Empty) == id);
            if (index != -1) _items[index] = element;
        }

        public void Remove(T element) => _items.Remove(element);

        public void Save(string filePath)
        {
            var json = JsonSerializer.Serialize(_items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public void Load(string filePath)
        {
            if (!File.Exists(filePath)) return;
            var json = File.ReadAllText(filePath);
            var items = JsonSerializer.Deserialize<List<T>>(json);
            if (items != null)
            {
                _items.Clear();
                _items.AddRange(items);
            }
        }
    }
}