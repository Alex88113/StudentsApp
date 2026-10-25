📘 StudentsApp — документация проекта
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
StudentsApp — учебное консольное приложение, демонстрирующее полный цикл CRUD-операций над двумя связанными сущностями: студент и группа.

Параметр	Значение
Тип приложения	Консольное (CLI)
Язык	C# 12
Платформа	.NET 8
База данных	SQLite (файл students.db)
ORM	sqlite-net-pcl
Интерфейс	Текстовое меню в терминале
Цель проекта: продемонстрировать навыки работы с реляционной БД через асинхронный ORM-провайдер, реализовать слоистую архитектуру и корректную обработку ошибок.

2. Технологический стек
Технология	Роль в проекте	Обоснование выбора
C# / .NET 8	Язык и платформа	Асинхронность из коробки, строгая типизация
SQLite	Встраиваемая СУБД	Не требует сервера, идеальна для учебного проекта
sqlite-net-pcl	ORM-провайдер	Лёгкий, асинхронный API (SQLiteAsyncConnection)
async/await	Асинхронные операции	Не блокирует поток во время I/O
3. Архитектура
Приложение разделено на четыре слоя с чёткой зоной ответственности.

Принципы разделения
Слой	Отвечает за	Не знает о
Models	Структуру таблиц, атрибуты [PrimaryKey]	SQL, подключении, UI
Repositories	CRUD, трансляцию вызовов в SQLite	Меню, вводе пользователя
Services	Подключение, создание таблиц	Бизнес-логике
Program.cs	Меню, ввод/вывод	SQL, соединении с БД
Ключевая идея: Program.cs никогда не обращается к SQLiteAsyncConnection напрямую — только через репозитории. Это позволяет заменить SQLite на PostgreSQL, не меняя точку входа.

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
Папка / файл	Назначение
Models/Group.cs	Модель группы → таблица Groups
Models/Student.cs	Модель студента → таблица Students
Repositories/GroupRepository.cs	CRUD для групп
Repositories/StudentRepository.cs	CRUD для студентов
Services/DatabaseService.cs	Подключение к SQLite, создание таблиц
Program.cs	Точка входа: меню и обработчики
StudentsApp.csproj	Файл проекта
students.db	База данных (создаётся при запуске)
README.md	Документация
5. Схема базы данных
Таблица Groups
Поле	Тип	Ограничения	Описание
Id	int	PK, autoincrement	Уникальный идентификатор
Name	string	NOT NULL	Название группы
Таблица Students
Поле	Тип	Ограничения	Описание
Id	int	PK, autoincrement	Уникальный идентификатор
FirstName	string	NOT NULL	Имя студента
LastName	string	NOT NULL	Фамилия студента
GroupId	int	FK → Groups.Id	Группа студента
Связь между таблицами

Тип связи: «один-ко-многим» (one-to-many). Одна группа содержит много студентов, каждый студент принадлежит одной группе.

6. Функциональность
6.1. Студенты
#	Действие	Метод репозитория	SQL-эквивалент
1	Показать всех	GetAllStudentsAsync()	SELECT * FROM Students
2	Найти по Id	GetByIdStudentAsync(id)	SELECT * FROM Students WHERE Id = @id
3	Добавить	AddStudentAsync(student)	INSERT INTO Students VALUES (...)
4	Обновить	UpdateStudentAsync(student)	UPDATE Students SET ... WHERE Id = @id
5	Удалить	DeleteStudentAsync(student)	DELETE FROM Students WHERE Id = @id
6.2. Группы
#	Действие	Метод репозитория	SQL-эквивалент
6	Показать всех	GetAllGroupsAsync()	SELECT * FROM Groups
7	Найти по Id	GetByIdGroupAsync(id)	SELECT * FROM Groups WHERE Id = @id
8	Добавить	AddGroupAsync(group)	INSERT INTO Groups VALUES (...)
9	Обновить	UpdateGroupAsync(group)	UPDATE Groups SET ... WHERE Id = @id
10	Удалить	DeleteGroupAsync(group)	DELETE FROM Groups WHERE Id = @id
7. Обработка ошибок
Ситуация	Механизм	Реакция приложения
Ввод текста вместо Id	int.TryParse	«Введите число», возврат в меню
Поиск несуществующей записи	FindAsync → null	«Студент с Id=X не найден»
Пустое имя/фамилия/название	string.IsNullOrWhiteSpace	«Поле не может быть пустым»
Несуществующий GroupId	GetByIdGroupAsync → null	«Группы с Id=X не существует»
Случайное удаление	Подтверждение y/n	«Отменено»
Ключевой принцип: приложение не падает при некорректном вводе — оно сообщает об ошибке и возвращается в меню.

8. Запуск
Требования
✅ .NET 8 SDK

✅ Windows / Linux / macOS

Команды
bash
# 1. Восстановить зависимости
dotnet restore

# 2. Собрать проект
dotnet build

# 3. Запустить
dotnet run
Первый запуск
При первом запуске автоматически:

Создаётся файл students.db в папке проекта.
Создаются таблицы Students и Groups.

10. Ограничения и улучшения
Текущие ограничения
Ограничение	Причина	Влияние
Нет каскадного удаления	Не реализовано	При удалении группы студенты остаются с «висящим» GroupId
Нет графического интерфейса	Учебная задача	Только консоль
Нет авторизации	Учебная задача	Любой пользователь имеет полный доступ
Нет юнит-тестов	Учебная задача	Ручное тестирование через меню
Возможные улучшения
Улучшение	Что даст
IRepository<T> — общий интерфейс	Унификация репозиториев, легче тестировать
Каскадное удаление	Целостность данных
Юнит-тесты (xUnit)	Автоматическая проверка CRUD
Переход на EF Core	Миграции, навигационные свойства, Include
Вывод группы по имени студента	Удобство: вместо GroupId показывать название группы
📎 Приложение: используемые методы sqlite-net-pcl
Метод	Назначение
InsertAsync(obj)	Вставка новой записи
Table<T>().ToListAsync()	Выборка всех записей
FindAsync<T>(pk)	Поиск по PK (возвращает null)
UpdateAsync(obj)	Обновление записи
DeleteAsync(obj)	Удаление записи
CreateTableAsync<T>()	Создание таблицы (идемпотентно)
