📘 StudentsApp — документация
Консольное приложение для учёта студентов и групп на C# (.NET 8) с хранением данных в SQLite.

📑 Содержание
О проекте

Технологический стек

Архитектура

Структура проекта

Схема базы данных

Функциональность

Обработка ошибок

Запуск

Сценарий демонстрации

Ограничения и улучшения

1. О проекте
StudentsApp — учебное консольное приложение с CRUD-операциями над студентами и группами.

Параметр	Значение
Тип	Консольное (CLI)
Язык	C# 12
Платформа	.NET 8
БД	SQLite
ORM	sqlite-net-pcl
UI	Меню в терминале
Цель: показать работу с БД через async ORM, слоистую архитектуру и обработку ошибок.

2. Технологический стек
Технология	Роль	Обоснование
C# / .NET 8	Язык и платформа	Async из коробки
SQLite	СУБД	Без сервера, один файл
sqlite-net-pcl	ORM	Лёгкий, async API
async/await	Асинхронность	Не блокирует поток
3. Архитектура
Приложение разделено на 4 слоя.

text
┌──────────────┐
│  Program.cs  │  меню, ввод/вывод
└──────┬───────┘
       │ вызывает
┌──────▼───────┐
│ Repositories │  CRUD, изоляция ORM
└──────┬───────┘
       │ используют
┌──────▼───────┐
│   Services   │  подключение, таблицы
└──────┬───────┘
       │ работает с
┌──────▼───────┐
│    Models    │  структура таблиц
└──────────────┘
Принципы разделения
Слой	Отвечает за	Не знает о
Models	Структуру таблиц	SQL, UI
Repositories	CRUD	Меню, вводе
Services	Подключение	Бизнес-логике
Program.cs	Меню, ввод/вывод	SQL, БД
Ключевая идея: Program.cs не обращается к SQLiteAsyncConnection напрямую — только через репозитории.

4. Структура проекта
text
StudentsApp/
├── Models/
│   ├── Group.cs
│   └── Student.cs
├── Repositories/
│   ├── GroupRepository.cs
│   └── StudentRepository.cs
├── Services/
│   └── DatabaseService.cs
├── Program.cs
├── StudentsApp.csproj
├── students.db
└── README.md

Файл	Назначение
Group.cs	Модель группы
Student.cs	Модель студента
GroupRepository.cs	CRUD групп
StudentRepository.cs	CRUD студентов
DatabaseService.cs	Подключение к БД
Program.cs	Меню и обработчики
students.db	База данных

6. Схема базы данных
Таблица Groups
Поле	Тип	Ограничения
Id	int	PK, autoincrement
Name	string	NOT NULL
Таблица Students
Поле	Тип	Ограничения
Id	int	PK, autoincrement
FirstName	string	NOT NULL
LastName	string	NOT NULL
GroupId	int	FK → Groups.Id

Связь
Groups (1) ──────< (N) Students
   Id  ◄────────────  GroupId
Тип: «один-ко-многим». Одна группа — много студентов.

7. Функциональность
6.1. Студенты
#	Действие	Метод	SQL
1	Все	GetAllStudentsAsync()	SELECT *
2	По Id	GetByIdStudentAsync(id)	WHERE Id=@id
3	Добавить	AddStudentAsync(s)	INSERT
4	Обновить	UpdateStudentAsync(s)	UPDATE
5	Удалить	DeleteStudentAsync(s)	DELETE
6.2. Группы
#	Действие	Метод	SQL
6	Все	GetAllGroupsAsync()	SELECT *
7	По Id	GetByIdGroupAsync(id)	WHERE Id=@id
8	Добавить	AddGroupAsync(g)	INSERT
9	Обновить	UpdateGroupAsync(g)	UPDATE
10	Удалить	DeleteGroupAsync(g)	DELETE
8. Обработка ошибок
Ситуация	Механизм	Реакция
Текст вместо Id	int.TryParse	«Введите число»
Нет записи	FindAsync → null	«Не найден»
Пустое поле	IsNullOrWhiteSpace	«Поле пустое»
Нет группы	GetByIdGroupAsync → null	«Группы нет»
Удаление	y/n	«Отменено»
Принцип: приложение не падает при некорректном вводе.

9. Запуск
Требования: .NET 8 SDK, Windows/Linux/macOS.

bash
dotnet restore
dotnet build
dotnet run

Первый запуск: создаётся students.db и таблицы Students, Groups.
The table Students is created.
The table Groups is created.

10. Ограничения и улучшения
Ограничения
Ограничение	Влияние
Нет каскадного удаления	«Висящий» GroupId
Нет GUI	Только консоль
Нет авторизации	Полный доступ
Нет тестов	Ручная проверка
Улучшения
Улучшение	Что даст
IRepository<T>	Унификация
Каскадное удаление	Целостность
xUnit-тесты	Автопроверка
EF Core	Миграции, Include
Имя группы вместо Id	Удобство

📎 Методы sqlite-net-pcl
Метод	Назначение
InsertAsync	Вставка
Table<T>().ToListAsync()	Все записи
FindAsync<T>(pk)	Поиск по PK (null)
UpdateAsync	Обновление
DeleteAsync	Удаление
CreateTableAsync<T>()	Создание таблицы
