<div align="center">

# 📘 StudentsApp

### Консольное приложение для учёта студентов и групп

![C#](https://img.shields.io/badge/C%23-12-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQLite](https://img.shields.io/badge/SQLite-003B57?style=for-the-badge&logo=sqlite&logoColor=white)
![License](https://img.shields.io/badge/license-MIT-blue?style=for-the-badge)

**Учебный проект · CRUD · Асинхронная работа с БД · Слоистая архитектура**

</div>

---

## 📑 Содержание

| Основное | Детали |
|:---------|:-------|
| [📖 О проекте](#-о-проекте) | [🗄 Схема базы данных](#-схема-базы-данных) |
| [🛠 Технологический стек](#-технологический-стек) | [⚙️ Функциональность](#️-функциональность) |
| [🏗 Архитектура](#-архитектура) | [🛡 Обработка ошибок](#-обработка-ошибок) |
| [📂 Структура проекта](#-структура-проекта) | [🚀 Запуск](#-запуск) |
| | [🔮 Ограничения и улучшения](#-ограничения-и-улучшения) |

---

## 📖 О проекте

**StudentsApp** — учебное консольное приложение, демонстрирующее полный цикл **CRUD-операций** над двумя связанными сущностями: *студент* и *группа*.

<table>
<tr>
<td width="50%" valign="top">

### 🎯 Цель

Продемонстрировать навыки работы с реляционной БД через асинхронный ORM-провайдер, реализовать слоистую архитектуру и корректную обработку ошибок.

</td>
<td width="50%" valign="top">

### 📊 Характеристики

| Параметр | Значение |
|----------|----------|
| Тип | Консольное (CLI) |
| Язык | C# 12 |
| Платформа | .NET 8 |
| БД | SQLite |
| ORM | sqlite-net-pcl |
| UI | Терминал |

</td>
</tr>
</table>

---

## 🛠 Технологический стек

| Технология | Роль | Обоснование |
|:-----------|:-----|:------------|
| **C# / .NET 8** | Язык и платформа | Асинхронность из коробки |
| **SQLite** | Встраиваемая СУБД | Без сервера, один файл |
| **sqlite-net-pcl** | ORM-провайдер | Лёгкий async API |
| **async/await** | Асинхронность | Не блокирует поток |

---

## 🏗 Архитектура

Приложение разделено на **четыре слоя** с чёткой зоной ответственности.

```mermaid
flowchart TD
    A["🖥 Program.cs<br/><i>меню, ввод/вывод</i>"]
    B["📦 Repositories<br/><i>CRUD, изоляция ORM</i>"]
    C["⚙️ Services<br/><i>подключение, таблицы</i>"]
    D["📋 Models<br/><i>структура таблиц</i>"]

    A -->|вызывает| B
    B -->|используют| C
    C -->|работает с| D

    style A fill:#512BD4,color:#fff
    style B fill:#239120,color:#fff
    style C fill:#003B57,color:#fff
    style D fill:#6c757d,color:#fff
🔑 Принципы разделения
Слой	Отвечает за	Не знает о
Models	Структуру таблиц	SQL, UI
Repositories	CRUD	Меню, вводе
Services	Подключение	Бизнес-логике
Program.cs	Меню	SQL, БД
💡 Program.cs не обращается к SQLiteAsyncConnection напрямую — только через репозитории.

📂 Структура проекта
text
StudentsApp/
│
├── 📂 Models/
│   ├── 📄 Group.cs
│   └── 📄 Student.cs
│
├── 📂 Repositories/
│   ├── 📄 GroupRepository.cs
│   └── 📄 StudentRepository.cs
│
├── 📂 Services/
│   └── 📄 DatabaseService.cs
│
├── 📄 Program.cs
├── 📄 StudentsApp.csproj
├── 📄 students.db
└── 📄 README.md
<details> <summary><b>📋 Назначение файлов</b></summary>
Файл	Назначение
Group.cs	Модель группы
Student.cs	Модель студента
GroupRepository.cs	CRUD групп
StudentRepository.cs	CRUD студентов
DatabaseService.cs	Подключение к БД
Program.cs	Меню и обработчики
students.db	База данных
</details>
🗄 Схема базы данных
<table> <tr> <td width="50%" valign="top">
📁 Groups
Поле	Тип	Ключ
Id	int	🔑 PK
Name	string	⚠️ NOT NULL
</td> <td width="50%" valign="top">
👤 Students
Поле	Тип	Ключ
Id	int	🔑 PK
FirstName	string	⚠️ NOT NULL
LastName	string	⚠️ NOT NULL
GroupId	int	🔗 FK → Groups
</td> </tr> </table>
🔗 Связь


















Тип: «один-ко-многим». Одна группа — много студентов.

⚙️ Функциональность
👤 Студенты
#	Действие	Метод
1	📋 Все	GetAllStudentsAsync()
2	🔍 По Id	GetByIdStudentAsync(id)
3	➕ Добавить	AddStudentAsync(s)
4	✏️ Обновить	UpdateStudentAsync(s)
5	🗑 Удалить	DeleteStudentAsync(s)
📁 Группы
#	Действие	Метод
6	📋 Все	GetAllGroupsAsync()
7	🔍 По Id	GetByIdGroupAsync(id)
8	➕ Добавить	AddGroupAsync(g)
9	✏️ Обновить	UpdateGroupAsync(g)
10	🗑 Удалить	DeleteGroupAsync(g)
🛡 Обработка ошибок
Ситуация	Механизм	Реакция
⌨️ Текст вместо Id	int.TryParse	«Введите число»
🔍 Нет записи	FindAsync → null	«Не найден»
📝 Пустое поле	IsNullOrWhiteSpace	«Поле пустое»
🔗 Нет группы	GetByIdGroupAsync → null	«Группы нет»
🗑 Удаление	y/n	«Отменено»
✅ Приложение не падает при некорректном вводе.

🚀 Запуск
✅ Требования
☑ .NET 8 SDK

☑ Windows / Linux / macOS

💻 Команды
bash
dotnet restore
dotnet build
dotnet run
🎉 Первый запуск
Создаются students.db и таблицы Students, Groups.

text
✅ The table Students is created.
✅ The table Groups is created.
🔮 Ограничения и улучшения
<table> <tr> <td width="50%" valign="top">
⚠️ Ограничения
Ограничение	Влияние
Нет каскада	«Висящий» GroupId
Нет GUI	Только консоль
Нет авторизации	Полный доступ
Нет тестов	Ручная проверка
</td> <td width="50%" valign="top">
🚀 Улучшения
Улучшение	Что даст
IRepository<T>	Унификация
Каскад	Целостность
xUnit	Автопроверка
EF Core	Миграции
</td> </tr> </table>
📎 Методы sqlite-net-pcl
Метод	Назначение
InsertAsync	➕ Вставка
Table<T>().ToListAsync()	📋 Все записи
FindAsync<T>(pk)	🔍 Поиск по PK
UpdateAsync	✏️ Обновление
DeleteAsync	🗑 Удаление
CreateTableAsync<T>()	🏗 Создание таблицы
<div align="center">
📘 StudentsApp
Учебный проект по курсу «Технология доступа к базам данных ADO.NET»

Сделано с ❤️ на C# / .NET 8 / SQLite

</div> ```
