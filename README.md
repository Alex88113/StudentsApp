📘 StudentsApp
Консольное приложение для учёта студентов и групп
Учебный проект · CRUD · Асинхронная работа с БД · Слоистая архитектура

</div>
📑 Содержание
<table> <tr> <td valign="top" width="50%">
Основное

📖 О проекте

🛠 Технологический стек

🏗 Архитектура

📂 Структура проекта

</td> <td valign="top" width="50%">
Детали

🗄 Схема базы данных

⚙️ Функциональность

🛡 Обработка ошибок

🚀 Запуск

🎬 Демонстрация

🔮 Ограничения и улучшения

</td> </tr> </table>
📖 О проекте
StudentsApp — учебное консольное приложение, демонстрирующее полный цикл CRUD-операций над двумя связанными сущностями: студент и группа.

<table> <tr> <td width="50%">
🎯 Цель
Продемонстрировать навыки работы с реляционной БД через асинхронный ORM-провайдер, реализовать слоистую архитектуру и корректную обработку ошибок.

</td> <td width="50%">
📊 Характеристики
Параметр	Значение
Тип	Консольное (CLI)
Язык	C# 12
Платформа	.NET 8
БД	SQLite (students.db)
ORM	sqlite-net-pcl
UI	Текстовое меню
</td> </tr> </table>
🛠 Технологический стек
<table> <tr> <th align="center">Технология</th> <th align="center">Роль</th> <th align="center">Обоснование</th> </tr> <tr> <td align="center"><b>C# / .NET 8</b></td> <td>Язык и платформа</td> <td>Асинхронность из коробки, строгая типизация</td> </tr> <tr> <td align="center"><b>SQLite</b></td> <td>Встраиваемая СУБД</td> <td>Не требует сервера, идеальна для учебного проекта</td> </tr> <tr> <td align="center"><b>sqlite-net-pcl</b></td> <td>ORM-провайдер</td> <td>Лёгкий, асинхронный API (<code>SQLiteAsyncConnection</code>)</td> </tr> <tr> <td align="center"><b>async/await</b></td> <td>Асинхронные операции</td> <td>Не блокирует поток во время I/O</td> </tr> </table>
🏗 Архитектура
Приложение разделено на четыре слоя с чёткой зоной ответственности.








🔑 Принципы разделения
<table> <tr> <th align="left">Слой</th> <th align="left">Отвечает за</th> <th align="left">Не знает о</th> </tr> <tr> <td><b>Models</b></td> <td>Структуру таблиц, атрибуты <code>[PrimaryKey]</code></td> <td>SQL, подключении, UI</td> </tr> <tr> <td><b>Repositories</b></td> <td>CRUD, трансляцию вызовов в SQLite</td> <td>Меню, вводе пользователя</td> </tr> <tr> <td><b>Services</b></td> <td>Подключение, создание таблиц</td> <td>Бизнес-логике</td> </tr> <tr> <td><b>Program.cs</b></td> <td>Меню, ввод/вывод</td> <td>SQL, соединении с БД</td> </tr> </table>
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
<table> <tr> <td valign="top" width="50%">
📁 Таблица Groups
Поле	Тип	Ограничения
Id	int	🔑 PK, autoincrement
Name	string	⚠️ NOT NULL
</td> <td valign="top" width="50%">
👤 Таблица Students
Поле	Тип	Ограничения
Id	int	🔑 PK, autoincrement
FirstName	string	⚠️ NOT NULL
LastName	string	⚠️ NOT NULL
GroupId	int	🔗 FK → Groups.Id
</td> </tr> </table>
🔗 Связь между таблицами
























Тип связи: «один-ко-многим» (one-to-many). Одна группа содержит много студентов, каждый студент принадлежит одной группе.

⚙️ Функциональность
👤 Студенты
#	Действие	Метод репозитория	SQL-эквивалент
1	📋 Показать всех	GetAllStudentsAsync()	SELECT * FROM Students
2	🔍 Найти по Id	GetByIdStudentAsync(id)	SELECT * FROM Students WHERE Id = @id
3	➕ Добавить	AddStudentAsync(student)	INSERT INTO Students VALUES (...)
4	✏️ Обновить	UpdateStudentAsync(student)	UPDATE Students SET ... WHERE Id = @id
5	🗑 Удалить	DeleteStudentAsync(student)	DELETE FROM Students WHERE Id = @id
📁 Группы
#	Действие	Метод репозитория	SQL-эквивалент
6	📋 Показать всех	GetAllGroupsAsync()	SELECT * FROM Groups
7	🔍 Найти по Id	GetByIdGroupAsync(id)	SELECT * FROM Groups WHERE Id = @id
8	➕ Добавить	AddGroupAsync(group)	INSERT INTO Groups VALUES (...)
9	✏️ Обновить	UpdateGroupAsync(group)	UPDATE Groups SET ... WHERE Id = @id
10	🗑 Удалить	DeleteGroupAsync(group)	DELETE FROM Groups WHERE Id = @id
🛡 Обработка ошибок
<table> <tr> <th align="left">Ситуация</th> <th align="left">Механизм</th> <th align="left">Реакция приложения</th> </tr> <tr> <td>⌨️ Ввод текста вместо Id</td> <td><code>int.TryParse</code></td> <td>«Введите число», возврат в меню</td> </tr> <tr> <td>🔍 Поиск несуществующей записи</td> <td><code>FindAsync</code> → <code>null</code></td> <td>«Студент с Id=X не найден»</td> </tr> <tr> <td>📝 Пустое имя/фамилия/название</td> <td><code>string.IsNullOrWhiteSpace</code></td> <td>«Поле не может быть пустым»</td> </tr> <tr> <td>🔗 Несуществующий <code>GroupId</code></td> <td><code>GetByIdGroupAsync</code> → <code>null</code></td> <td>«Группы с Id=X не существует»</td> </tr> <tr> <td>🗑 Случайное удаление</td> <td>Подтверждение <code>y/n</code></td> <td>«Отменено»</td> </tr> </table>
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
🎬 Демонстрация
Шаг	Действие	Ожидаемый результат
1	🚀 Запуск dotnet run	Меню с 10 пунктами
2	8 → «ПИ-21-1»	✅ «Группа добавлена»
3	7 → Id=1	📁 Показ группы «ПИ-21-1»
4	3 → «Александр», «Кучерявых», GroupId=1	✅ «Студент добавлен»
5	1	📋 Список с одним студентом
6	4 → Id=1 → новое имя	✏️ «Студент обновлён»
7	2 → Id=1	🔍 Обновлённое имя
8	2 → Id=999	⚠️ «Студент с Id=999 не найден» (без падения)
9	5 → Id=1 → y	🗑 «Студент удалён»
10	Открыть students.db в DB Browser	💾 Данные сохранены в таблицах
💡 Акцент на шаге 8: демонстрация корректной обработки ошибки — преподаватели это ценят.

🔮 Ограничения и улучшения
<table> <tr> <td valign="top" width="50%">
⚠️ Текущие ограничения
Ограничение	Влияние
Нет каскадного удаления	Студенты остаются с «висящим» GroupId
Нет GUI	Только консоль
Нет авторизации	Полный доступ у всех
Нет юнит-тестов	Ручное тестирование
</td> <td valign="top" width="50%">
🚀 Возможные улучшения
Улучшение	Что даст
IRepository<T>	Унификация, тестируемость
Каскадное удаление	Целостность данных
xUnit-тесты	Автопроверка CRUD
EF Core	Миграции, Include
Вывод группы по имени	Вместо GroupId — название
</td> </tr> </table>
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
