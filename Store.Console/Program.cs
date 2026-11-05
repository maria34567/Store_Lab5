using Microsoft.EntityFrameworkCore;
using Store.Common;
using Store.Infrastructure;
using Store.Infrastructure.Models;
using Store.Infrastructure.Repositories;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("=== ЛАБОРАТОРНА РОБОТА №3: ENTITY FRAMEWORK CORE ===");
var dbPath = Path.GetFullPath("store_lab3.db");
var options = new DbContextOptionsBuilder<StoreContext>().UseSqlite($"Data Source={dbPath}").Options;
await using var context = new StoreContext(options);
await context.Database.MigrateAsync();
if (!await context.Categories.AnyAsync())
{
    context.Categories.Add(new CategoryModel { Id=1, Name="Електроніка" });
    await context.SaveChangesAsync();
}
if (!await context.Suppliers.AnyAsync())
{
    context.Suppliers.AddRange(new SupplierModel { Id=1, Name="Постачальник А" }, new SupplierModel { Id=2, Name="Постачальник Б" });
    await context.SaveChangesAsync();
}
var service = new ElectronicsCrudService(new GuidRepository<ElectronicsModel>(context));
if (!(await service.ReadAllAsync()).Any())
{
    // DbContext is not thread-safe: generation is parallel; DB inserts are sequential and async.
    var generated = new System.Collections.Concurrent.ConcurrentBag<Electronics>();
    Parallel.For(0, 1000, _ => generated.Add(Electronics.CreateNew()));
    foreach(var item in generated) await service.CreateAsync(item);
}
var items = (await service.ReadAllAsync()).ToArray();
Console.WriteLine($"SQLite: {dbPath}");
Console.WriteLine($"Усього товарів: {items.Length}");
Console.WriteLine($"Ціна: min={items.Min(x=>x.Price):F2}, max={items.Max(x=>x.Price):F2}, avg={items.Average(x=>x.Price):F2}");
Console.WriteLine($"Гарантія: min={items.Min(x=>x.WarrantyMonths)}, max={items.Max(x=>x.WarrantyMonths)}, avg={items.Average(x=>x.WarrantyMonths):F2}");
Console.WriteLine("Перша сторінка (5 товарів):");
foreach(var item in await service.ReadAllAsync(1,5)) Console.WriteLine(item.GetInfo());
var first = items[0];
Console.WriteLine($"ReadAsync: {(await service.ReadAsync(first.Id)).Name}");
var updated = new Electronics("Оновлений товар", 9999m, "Acer", 24) { Id=first.Id };
Console.WriteLine($"UpdateAsync: {await service.UpdateAsync(updated)}");
Console.WriteLine($"RemoveAsync: {await service.RemoveAsync(updated)}");
Console.WriteLine($"CreateAsync: {await service.CreateAsync(updated)}");
if (!await context.ProductDetails.AnyAsync())
{
    context.ProductDetails.Add(new ProductDetailsModel { ProductId=first.Id, Description="Гарантійний товар" });
    context.ProductSuppliers.Add(new ProductSupplierModel { ProductId=first.Id, SupplierId=1 });
    context.ProductSuppliers.Add(new ProductSupplierModel { ProductId=first.Id, SupplierId=2 });
    await context.SaveChangesAsync();
}
Console.WriteLine($"Категорій: {await context.Categories.CountAsync()}, постачальників: {await context.Suppliers.CountAsync()}");
Console.WriteLine($"Деталей: {await context.ProductDetails.CountAsync()}, зв'язків товар-постачальник: {await context.ProductSuppliers.CountAsync()}");
Console.WriteLine($"SaveAsync: {await service.SaveAsync()}");
