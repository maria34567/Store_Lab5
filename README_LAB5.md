# Лабораторна робота №5 — ASP.NET Core Identity

## Реалізовано
- StoreUser : IdentityUser, IdentityDbContext<StoreUser>, Identity EF Core package.
- Built-in Identity bearer endpoints (`POST /login?useCookies=false`, `/refresh`), custom `POST /api/accounts/register` with default Customer role.
- Roles: Customer, Manager, Admin. Public GET, authorized POST, Manager/Admin PUT, Admin DELETE.
- `GET /api/accounts/me`, admin-only `POST /api/accounts/assign-role`.

## ВАЖЛИВО: міграція Identity
У цьому середовищі .NET SDK недоступний, тому нову EF міграцію **потрібно згенерувати локально**. Не запускайте REST до цього кроку.

У корені проєкту, маючи .NET 10 SDK:

```powershell
dotnet tool install --global dotnet-ef --version 10.0.0
# Якщо вже встановлений: dotnet tool update --global dotnet-ef --version 10.0.0
dotnet ef migrations add AddIdentity --project Store.Infrastructure --startup-project Store.REST
dotnet ef database update --project Store.Infrastructure --startup-project Store.REST
```

Перевірте, що міграція містить AspNetUsers, AspNetRoles, AspNetUserRoles тощо. Додайте файли міграції до Git.

Для першого адміністратора задайте **тимчасові локальні** змінні середовища PowerShell:

```powershell
$env:STORE_ADMIN_EMAIL = "admin@example.local"
$env:STORE_ADMIN_PASSWORD = "ReplaceWithStrongUniquePassword!123"
dotnet run --project Store.REST/Store.REST.csproj --launch-profile http
```

Пароль у прикладі замініть; не додавайте справжні паролі в Git. Після першого запуску приберіть змінні середовища (`Remove-Item Env:STORE_ADMIN_PASSWORD`, аналогічно email). Для існуючого облікового запису роль Admin автоматично НЕ видається.

## Перевірка запитів
- `GET /api/products` без токена → 200.
- `POST /api/accounts/register` з JSON `{ "email":"customer@example.com", "password":"StrongPass!123", "displayName":"Student" }` → 201.
- `POST /login?useCookies=false` з JSON `{ "email":"customer@example.com", "password":"StrongPass!123" }` → `accessToken`.
- `GET /api/accounts/me` з `Authorization: Bearer <accessToken>` → 200; без токена → 401.
- `PUT /api/products/{id}` з Customer токеном → 403; з Manager токеном → 204 (за наявності товару).
- `DELETE /api/products/{id}` з Manager токеном → 403; з Admin токеном → 204 (за наявності товару).
- `POST /api/accounts/assign-role` доступний лише Admin.

Тестуйте через Postman або Swagger (для Swagger bearer заголовок може знадобитися вводити вручну або налаштувати security definition).

## Звіт та Git
Зробіть справжні скриншоти запитів: реєстрація, логін із **замаскованим** токеном, 200, 201, 401, 403, 204. Додайте PDF у PR. Створіть гілку `lab5` від `lab4`, якщо PR lab4 ще відкритий. Хмарне розгортання — окреме додаткове завдання, не виконано.

**Статус:** код підготовлено, але компіляція, міграція і HTTP-тести локально ще не виконані. Перед здачею обов'язково перевірте `dotnet build`, `dotnet run` і всі сценарії.
