📘 StudentsApp
Консольное приложение для учёта студентов и групп

Учебный проект · CRUD · Асинхронная работа с БД · Слоистая архитектура

📑 Содержание
Основное	Детали
📖 О проекте	🗄 Схема базы данных
🛠 Технологический стек	⚙️ Функциональность
🏗 Архитектура	🛡 Обработка ошибок
📂 Структура проекта	🚀 Запуск
🔮 Ограничения и улучшения
📖 О проекте
StudentsApp — учебное консольное приложение, демонстрирующее полный цикл CRUD-операций над двумя связанными сущностями: студент и группа.

🎯 Цель
Продемонстрировать навыки работы с реляционной БД через асинхронный ORM-провайдер, реализовать слоистую архитектуру и корректную обработку ошибок.

📊 Характеристики
Параметр	Значение
Тип	Консольное (CLI)
Язык	C# 12
Платформа	.NET 8
БД	SQLite (students.db)
ORM	sqlite-net-pcl
UI	Текстовое меню
🛠 Технологический стек
Технология	Роль	Обоснование
C# / .NET 8	Язык и платформа	Асинхронность из коробки, строгая типизация
SQLite	Встраиваемая СУБД	Не требует сервера, идеальна для учебного проекта
sqlite-net-pcl	ORM-провайдер	Лёгкий, асинхронный API (SQLiteAsyncConnection)
async/await	Асинхронные операции	Не блокирует поток во время I/O
🏗 Архитектура
Приложение разделено на четыре слоя с чёткой зоной ответственности.

text
┌─────────────────────────────────────────────┐
│  Program.cs                                 │
│  точка входа, меню, ввод/вывод              │
└────────────────────┬────────────────────────┘
                     │ вызывает
┌────────────────────▼────────────────────────┐
│  Repositories/                              │
│  GroupRepository · StudentRepository        │
│  CRUD-операции                              │
└────────────────────┬────────────────────────┘
                     │ используют
┌────────────────────▼────────────────────────┐
│  Services/                                  │
│  Database                                   │
│  подключение, создание таблиц               │
└────────────────────┬────────────────────────┘
                     │ работает с
┌────────────────────▼────────────────────────┐
│  Models/                                    │
│  Groups · Students                          │
│  структура таблиц                           │
└─────────────────────────────────────────────┘
🔑 Принципы разделения
Слой	Отвечает за	Не знает о
Models	Структуру таблиц, атрибуты [PrimaryKey]	SQL, подключении, UI
Repositories	CRUD, трансляцию вызовов в SQLite	Меню, вводе пользователя
Services	Подключение, создание таблиц	Бизнес-логике
Program.cs	Меню, ввод/вывод	SQL, соединении с БД
💡 Ключевая идея: Program.cs никогда не обращается к SQLiteAsyncConnection напрямую — только через репозитории. Это позволяет заменить SQLite на PostgreSQL, не меняя точку входа.

📂 Структура проекта
text
StudentsApp/
│
├── 📂 Models/                    # Модели данных
│   ├── 📄 Group.cs               # → таблица Groups
│   └── 📄 Student.cs             # → таблица Students
│
├── 📂 Repositories/              # Слой доступа к данным
│   ├── 📄 GroupRepository.cs     # CRUD для групп
│   └── 📄 StudentRepository.cs   # CRUD для студентов
│
├── 📂 Services/                  # Инфраструктурный слой
│   └── 📄 DatabaseService.cs     # Подключение и создание таблиц
│
├── 📄 Program.cs                 # Точка входа
├── 📄 StudentsApp.csproj         # Файл проекта
├── 📄 students.db                # База данных
└── 📄 README.md                  # Документация
<details> <summary><b>📋 Назначение файлов (нажми, чтобы развернуть)</b></summary>
Файл	Назначение
Models/Group.cs	Модель группы → таблица Groups
Models/Student.cs	Модель студента → таблица Students
Repositories/GroupRepository.cs	CRUD для групп
Repositories/StudentRepository.cs	CRUD для студентов
Services/DatabaseService.cs	Подключение к SQLite, создание таблиц
Program.cs	Точка входа: меню и обработчики
StudentsApp.csproj	Файл проекта
students.db	База данных (создаётся при запуске)
</details>
🗄 Схема базы данных
📁 Таблица Groups
Поле	Тип	Ограничения
Id	int	🔑 PK, autoincrement
Name	string	⚠️ NOT NULL
👤 Таблица Students
Поле	Тип	Ограничения
Id	int	🔑 PK, autoincrement
FirstName	string	⚠️ NOT NULL
LastName	string	⚠️ NOT NULL
GroupId	int	🔗 FK → Groups.Id
🔗 Связь между таблицами
text
Groups (1) ──────────< (N) Students
   Id  ◄────────────────────  GroupId
Тип связи: «один-ко-многим» (one-to-many). Одна группа содержит много студентов, каждый студент принадлежит одной группе.

⚙️ Функциональность
👤 Студенты
#	Действие	Метод репозитория	SQL-эквивалент
1	📋 Показать всех	GetAllStudentsAsync()	SELECT * FROM Students
2	🔍 Найти по Id	GetByIdStudentAsync(id)	SELECT ... WHERE Id = @id
3	➕ Добавить	AddStudentAsync(student)	INSERT INTO Students ...
4	✏️ Обновить	UpdateStudentAsync(student)	UPDATE Students SET ...
5	🗑 Удалить	DeleteStudentAsync(student)	DELETE FROM Students ...
📁 Группы
#	Действие	Метод репозитория	SQL-эквивалент
6	📋 Показать всех	GetAllGroupsAsync()	SELECT * FROM Groups
7	🔍 Найти по Id	GetByIdGroupAsync(id)	SELECT ... WHERE Id = @id
8	➕ Добавить	AddGroupAsync(group)	INSERT INTO Groups ...
9	✏️ Обновить	UpdateGroupAsync(group)	UPDATE Groups SET ...
10	🗑 Удалить	DeleteGroupAsync(group)	DELETE FROM Groups ...
🛡 Обработка ошибок
Ситуация	Механизм	Реакция приложения
⌨️ Ввод текста вместо Id	int.TryParse	«Введите число», возврат в меню
🔍 Поиск несуществующей записи	FindAsync → null	«Студент с Id=X не найден»
📝 Пустое имя/фамилия	string.IsNullOrWhiteSpace	«Поле не может быть пустым»
🔗 Несуществующий GroupId	GetByIdGroupAsync → null	«Группы с Id=X не существует»
🗑 Случайное удаление	Подтверждение y/n	«Отменено»
✅ Ключевой принцип: приложение не падает при некорректном вводе — оно сообщает об ошибке и возвращается в меню.

🚀 Запуск
✅ Требования
☑ .NET 8 SDK

☑ Windows / Linux / macOS

💻 Команды
bash
# 1. Восстановить зависимости
dotnet restore

# 2. Собрать проект
dotnet build

# 3. Запустить
dotnet run
🎉 Первый запуск
При первом запуске автоматически:

Создаётся файл students.db в папке проекта.

Создаются таблицы Students и Groups.

text
✅ The table Students is created.
✅ The table Groups is created.
🔮 Ограничения и улучшения
⚠️ Текущие ограничения
Ограничение	Влияние
Нет каскадного удаления	Студенты остаются с «висящим» GroupId
Нет GUI	Только консоль
Нет авторизации	Полный доступ у всех
Нет юнит-тестов	Ручное тестирование
🚀 Возможные улучшения
Улучшение	Что даст
IRepository<T>	Унификация, тестируемость
Каскадное удаление	Целостность данных
xUnit-тесты	Автопроверка CRUD
EF Core	Миграции, Include
Вывод группы по имени	Вместо GroupId — название
📎 Приложение
Методы sqlite-net-pcl, используемые в проекте
Метод	Назначение
InsertAsync(obj)	➕ Вставка новой записи
Table<T>().ToListAsync()	📋 Выборка всех записей
FindAsync<T>(pk)	🔍 Поиск по PK (возвращает null)
UpdateAsync(obj)	✏️ Обновление записи
DeleteAsync(obj)	🗑 Удаление записи
CreateTableAsync<T>()	🏗 Создание таблицы (идемпотентно)
<div align="center">
📘 StudentsApp
Учебный проект по курсу «Технология доступа к базам данных ADO.NET»

Сделано с ❤️ на C# / .NET 8 / SQLite

</div>
