📘 StudentsApp
Консольное приложение для учёта студентов и групп на C# (.NET 8) с SQLite.

📑 Содержание
О проекте

Технологии

Архитектура

Структура

Схема БД

CRUD-операции

Ошибки

Запуск

Ограничения

1. О проекте
StudentsApp — учебное консольное приложение с CRUD-операциями над студентами и группами.

Параметры:

Тип: консольное (CLI)

Язык: C# 12

Платформа: .NET 8

БД: SQLite (students.db)

ORM: sqlite-net-pcl

UI: текстовое меню

Цель: показать работу с БД через async ORM, слоистую архитектуру и обработку ошибок.

2. Технологии
C# / .NET 8 — язык и платформа, async из коробки

SQLite — встраиваемая СУБД, без сервера

sqlite-net-pcl — лёгкий async ORM

async/await — не блокирует поток

3. Архитектура
Четыре слоя:

Program.cs — меню, ввод/вывод

Repositories/ — CRUD, изоляция ORM

Services/ — подключение, создание таблиц

Models/ — структура таблиц

Принципы:

Models — структура таблиц, не знает про SQL и UI

Repositories — CRUD, не знает про меню

Services — подключение, не знает про бизнес-логику

Program.cs — меню, не знает про SQL

Program.cs не обращается к SQLiteAsyncConnection напрямую — только через репозитории.

4. Структура
text
StudentsApp/
  Models/
    Group.cs
    Student.cs
  Repositories/
    GroupRepository.cs
    StudentRepository.cs
  Services/
    DatabaseService.cs
  Program.cs
  StudentsApp.csproj
  students.db
  README.md
Назначение:

Group.cs — модель группы

Student.cs — модель студента

GroupRepository.cs — CRUD групп

StudentRepository.cs — CRUD студентов

DatabaseService.cs — подключение к БД

Program.cs — меню

students.db — база данных

5. Схема БД
Таблица Groups:

Id — int, PK, autoincrement

Name — string, NOT NULL

Таблица Students:

Id — int, PK, autoincrement

FirstName — string, NOT NULL

LastName — string, NOT NULL

GroupId — int, FK → Groups.Id

Связь: один-ко-многим (одна группа — много студентов).

6. CRUD-операции
Студенты:

Показать всех — GetAllStudentsAsync

Найти по Id — GetByIdStudentAsync

Добавить — AddStudentAsync

Обновить — UpdateStudentAsync

Удалить — DeleteStudentAsync

Группы:

Показать всех — GetAllGroupsAsync

Найти по Id — GetByIdGroupAsync

Добавить — AddGroupAsync

Обновить — UpdateGroupAsync

Удалить — DeleteGroupAsync

7. Ошибки
Текст вместо Id — int.TryParse — «Введите число»

Нет записи — FindAsync → null — «Не найден»

Пустое поле — IsNullOrWhiteSpace — «Поле пустое»

Нет группы — GetByIdGroupAsync → null — «Группы нет»

Удаление — подтверждение y/n — «Отменено»

Приложение не падает при некорректном вводе.

8. Запуск
Требования: .NET 8 SDK.

text
dotnet restore
dotnet build
dotnet run
При первом запуске создаются students.db и таблицы Students, Groups.

9. Ограничения
Нет каскадного удаления — «висящий» GroupId

Нет GUI — только консоль

Нет авторизации — полный доступ

Нет тестов — ручная проверка

Улучшения:

IRepository<T> — унификация

Каскадное удаление — целостность

xUnit — автопроверка

EF Core — миграции

📎 Методы sqlite-net-pcl
InsertAsync — вставка

Table<T>().ToListAsync — все записи

FindAsync — поиск по PK

UpdateAsync — обновление

DeleteAsync — удаление

CreateTableAsync — создание таблицы
