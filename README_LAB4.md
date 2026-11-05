# Лабораторна робота №4 — ASP.NET Core REST API

## Структура
- `Store.REST` — Web API, DTO у `Models`, контролери `Products` та `Categories`, Swagger.
- `Store.Infrastructure` — Entity Framework Core, SQLite, репозиторії та CRUD-сервіс лабораторної №3.
- `Store.MVC` — додаткове завдання: MVC-каталог із переглядом деталей товару.

## Запуск
Потрібен .NET 10 SDK і доступ до NuGet під час першого відновлення пакетів.

```powershell
dotnet restore Store.REST/Store.REST.csproj
dotnet run --project Store.REST/Store.REST.csproj --launch-profile http
```
Відкрити `http://localhost:5084/swagger`. База `store_lab3.db` створюється у корені проєкту під час першого запуску, якщо її немає. Категорія з ID=1 додається автоматично. Для 1000 товарів спочатку запустити `dotnet run --project Store.Console/Store.Console.csproj` у корені проєкту.

## Тестування через Swagger
1. `GET /api/categories` — 200.
2. `POST /api/categories` з `{ "name": "Аксесуари" }` — 201.
3. `GET /api/products?page=1&amount=5` — 200.
4. `POST /api/products` з `{ "name":"Монітор", "price":12000, "brand":"Acer", "warrantyMonths":24 }` — 201; скопіювати `id` з відповіді.
5. `GET /api/products/{id}` — 200.
6. `PUT /api/products/{id}` з `{ "name":"Монітор 27", "price":13000, "brand":"Acer", "warrantyMonths":36 }` — 204.
7. `DELETE /api/products/{id}` — 204; повторний GET — 404.
8. Невалідні дані POST — 400.

## MVC (додаткове)
```powershell
dotnet run --project Store.MVC/Store.MVC.csproj --launch-profile http
```
Відкрити `http://localhost:5085`.

## Git
Створіть гілку `lab4` від `lab3`, скопіюйте нові проєкти та файли до клонованого репозиторію, додайте PDF із **реальними** скриншотами запитів, потім `git add .`, `git commit -m "Lab 4 REST API and MVC"`, `git push -u origin lab4`.

## Звіт
Додайте скриншоти Swagger для GET, POST (201), PUT (204), DELETE (204), GET відсутнього ID (404), обох контролерів і, за бажанням, сторінки MVC. Скриншоти повинні бути зроблені після фактичного запуску.

## Зауваження
Компіляцію та HTTP-тести потрібно виконати на вашому ПК. Проєкт використовує версії EF Core, наявні в отриманому архіві. Перед здачею перевірте `dotnet list Store.Infrastructure/Store.Infrastructure.csproj package --vulnerable --include-transitive`.
