# Лабораторна №2

Проєкт збережено на базі лабораторної №1. Додані асинхронний CRUD, thread safety, LINQ, Parallel.For, пагінація, IEnumerable, синхронізація, JSON та xUnit-тести.

## Запуск

Потрібен .NET 10 SDK.

```bash
dotnet run --project Store.Console/Store.Console.csproj
dotnet test Store.Lab2.Tests/Store.Lab2.Tests.csproj
```

## GitHub

На комп'ютері у клонованому репозиторії виконайте `git switch -c lab2` (від гілки з першою лабораторною), перенесіть оновлені файли, далі `git add .`, `git commit -m "Implement lab2 async CRUD"`, `git push -u origin lab2`. Створіть Pull Request у `master` або `lab1`, якщо PR першої роботи ще відкритий.

## Звіт

Запустіть застосунок, зробіть скриншот консолі, додайте його до PDF та до Pull Request. Файл з фактичними результатами виконання неможливо створити без запуску .NET програми.
