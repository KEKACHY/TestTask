# TestTask

WebAPI приложение для обработки CSV данных и хранения результатов анализа.

## Stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Swagger

## Run

1. Создать БД PostgreSQL

2. Настроить connection string

3. Выполнить:

dotnet ef database update

4. Запустить приложение

Swagger:
https://localhost:7004/swagger

## API

POST /api/results/upload

GET /api/results

GET /api/results/{fileName}/values
