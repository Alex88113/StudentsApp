📘 StudentsApp — документация
Консольное приложение для учёта студентов и групп на C# (.NET 8) с хранением данных в SQLite.

📑 Содержание
О проекте

Технологии

Архитектура

Структура

Схема БД

CRUD-операции

Ошибки

Запуск

Демонстрация

Ограничения

1. О проекте
StudentsApp — консольное приложение
для учёта студентов и групп.

Параметр	Значение
Тип	CLI
Язык	C# 12
Платформа	.NET 8
БД	SQLite
ORM	sqlite-net-pcl
UI	Терминал
Цель: показать CRUD через async ORM,
слоистую архитектуру и обработку ошибок.

2. Технологии
Технология	Роль
C# / .NET 8	Язык и платформа
SQLite	Встраиваемая СУБД
sqlite-net-pcl	ORM-провайдер
async/await	Асинхронность
3. Архитектура
Четыре слоя с чёткой ответственностью.

text
Program.cs
  │ меню, ввод/вывод
  ▼
Repositories/
  │ CRUD, изоляция ORM
  ▼
Services/
  │ подключение, таблицы
  ▼
Models/
  │ структура таблиц
Принципы
Слой	Отвечает за
Models	Структуру таблиц
Repositories	CRUD
Services	Подключение
Program.cs	Меню
Program.cs не обращается к БД напрямую —
только через репозитории.

4. Структура
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
Program.cs	Меню
students.db	База данных

5. Схема БД
Groups
Поле	Тип	Ключ
Id	int	PK
Name	string	—
Students
Поле	Тип	Ключ
Id	int	PK
FirstName	string	—
LastName	string	—
GroupId	int	FK → Groups

Связь
Groups (1) ────< (N) Students
  Id  ◄──────────  GroupId
Тип: «один-ко-многим».

6. CRUD-операции
Студенты
#	Действие	Метод
1	Все	GetAllStudentsAsync
2	По Id	GetByIdStudentAsync
3	Добавить	AddStudentAsync
4	Обновить	UpdateStudentAsync
5	Удалить	DeleteStudentAsync
Группы
#	Действие	Метод
6	Все	GetAllGroupsAsync
7	По Id	GetByIdGroupAsync
8	Добавить	AddGroupAsync
9	Обновить	UpdateGroupAsync
10	Удалить	DeleteGroupAsync
7. Ошибки
Ситуация	Реакция
Текст вместо Id	«Введите число»
Нет записи	«Не найден»
Пустое поле	«Поле пустое»
Нет группы	«Группы нет»
Удаление	«Отменено»
Приложение не падает
при некорректном вводе.

8. Запуск
Требования: .NET 8 SDK.

bash
dotnet restore
dotnet build
dotnet run
При первом запуске создаются
students.db и таблицы.

The table Students is created.
The table Groups is created.

10. Ограничения
Ограничение	Влияние
Нет каскада	«Висящий» GroupId
Нет GUI	Только консоль
Нет авторизации	Полный доступ
Нет тестов	Ручная проверка
Улучшения
Улучшение	Что даст
IRepository<T>	Унификация
Каскад	Целостность
xUnit	Автопроверка
EF Core	Миграции
📎 Методы sqlite-net-pcl
Метод	Назначение
InsertAsync	Вставка
Table<T>().ToListAsync	Все записи
FindAsync	Поиск по PK
UpdateAsync	Обновление
DeleteAsync	Удаление
CreateTableAsync	Создание таблицы
