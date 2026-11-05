# Лабораторна №3 — Store / Entity Framework Core

Це продовження лабораторної №2. Збережено Store.Common та Store.Lab2.Tests; додано Store.Infrastructure із моделями, Fluent API, TPT, міграцією, IRepository<T> та CRUD на SQLite. Консоль тепер використовує базу даних.

## Запуск (потрібен .NET 10 SDK)

З кореня репозиторію:

```powershell
dotnet restore Store.Console/Store.Console.csproj
dotnet run --project Store.Console/Store.Console.csproj
dotnet test Store.Lab2.Tests/Store.Lab2.Tests.csproj
```

Після першого запуску створиться `store_lab3.db` у **поточній робочій папці**. Файл потрібно додати до Git (SQLite є вимогою завдання). Якщо `.gitignore` ігнорує `*.db`, додай примусово `git add -f store_lab3.db`.

Для роботи з EF CLI (необов'язково для запуску):

```powershell
dotnet tool install --global dotnet-ef --version 10.0.0
dotnet ef migrations list --project Store.Infrastructure --startup-project Store.Console
dotnet ef database update --project Store.Infrastructure --startup-project Store.Console
```

Початкова міграція додана вручну за схемою Fluent API; **перевірте її на своєму комп'ютері**. Для повного автогенерованого snapshot можна видалити вручну додану міграцію та створити її заново командою `dotnet ef migrations add InitialCreate --project Store.Infrastructure --startup-project Store.Console` до першого запуску (або в чистій копії проєкту). Не запускайте `migrations add` із такою ж назвою поверх існуючої міграції.

## Скриншоти для звіту

1. Консоль із результатами `dotnet run`.
2. Відкрий `store_lab3.db` у DB Browser for SQLite; зроби скриншот списку таблиць.
3. Зроби скриншоти вмісту `Products`, `Electronics`, `Categories`, `ProductDetails`, `Suppliers`, `ProductSuppliers`.
4. Додай ERD із `ERD.md` (Mermaid preview у GitHub або редакторі).
5. Збережи скриншоти в PDF та додай PDF у гілку `lab3`.

## Важливі пояснення

- `Product` у першій лабораторній має `Guid Id`, а вимога `IRepository<T>` задає `GetByIdAsync(int id)`. Тому для категорій і постачальників є звичайний `EfRepository<T>`, а для товарів — `IGuidRepository<T>` із `GetByGuidAsync(Guid)`.
- `DbContext` не потокобезпечний. Генерація 1000 товарів виконується через `Parallel.For`, але запис у SQLite послідовний із `await`.
- `SaveAsync` повертає `true`, оскільки репозиторій виконує `SaveChangesAsync()` після кожної зміни.
- Файл БД та реальні скриншоти можна отримати тільки після запуску програми на комп'ютері.
- Додаткове завдання MongoDB не включено.
