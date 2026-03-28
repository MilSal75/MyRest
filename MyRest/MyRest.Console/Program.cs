#nullable disable
using System;
using System.Linq;
using MyRest.Domain.ValueObjects;
using MyRest.Domain.Entities;
using MyRest.Domain.Enums;

namespace MyRest.App;

internal class Program
{
    static Manager manager;

    static void Main(string[] args)
    {
        manager = DataService.Load();

        if (manager == null)
        {
            var managerPhone = new Phone("+79161234567");
            manager = new Manager("Иван Петрович", managerPhone);
            Console.WriteLine("Создан новый менеджер.");
        }

        Console.WriteLine("=== Управление персоналом ресторана ===\n");
        Console.WriteLine("Выберите роль:");
        Console.WriteLine("1. Менеджер");
        Console.WriteLine("2. Сотрудник");
        Console.Write("Ваш выбор: ");
        string roleChoice = Console.ReadLine();

        if (roleChoice == "2")
        {
            RunEmployeeMode();
        }
        else
        {
            RunManagerMode();
        }
    }

    static void RunManagerMode()
    {
        bool exit = false;
        while (!exit)
        {
            ShowManagerMenu();
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": ShowAllEmployees(); break;
                case "2": AddEmployee(); break;
                case "3": AssignShiftToEmployee(); break;
                case "4": RemoveShiftFromEmployee(); break;
                case "5": ShowVacationRequests(); break;
                case "6": ApproveVacation(); break;
                case "7": RejectVacation(); break;
                case "0": exit = true; Console.WriteLine("До свидания!"); break;
                default: Console.WriteLine("Неверный выбор."); break;
            }
        }
        Main(null);
    }

    static void RunEmployeeMode()
    {
        var employees = manager.Employees.ToList();
        if (employees.Count == 0)
        {
            Console.WriteLine("Нет сотрудников. Сначала добавьте сотрудников через меню менеджера.");
            return;
        }

        Console.WriteLine("\nВыберите себя из списка:");
        for (int i = 0; i < employees.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {employees[i].FullName} | {employees[i].Phone.Value}");
        }
        Console.Write("Введите номер: ");
        if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > employees.Count)
        {
            Console.WriteLine("Неверный выбор.");
            return;
        }

        var currentEmployee = employees[index - 1];

        bool exit = false;
        while (!exit)
        {
            ShowEmployeeMenu(currentEmployee.FullName);
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": ShowMyShifts(currentEmployee); break;
                case "2": RequestMyVacation(currentEmployee); break;
                case "3": MarkMyShift(currentEmployee); break;
                case "0": exit = true; break;
                default: Console.WriteLine("Неверный выбор."); break;
            }
        }
        Main(null);
    }

    static void ShowManagerMenu()
    {
        Console.WriteLine("\n--- Меню менеджера ---");
        Console.WriteLine("1. Показать всех сотрудников");
        Console.WriteLine("2. Добавить сотрудника");
        Console.WriteLine("3. Назначить смену");
        Console.WriteLine("4. Удалить смену");
        Console.WriteLine("5. Показать заявки на отпуск");
        Console.WriteLine("6. Одобрить отпуск");
        Console.WriteLine("7. Отклонить отпуск");
        Console.WriteLine("0. Выход");
        Console.Write("Выберите действие: ");
    }

    static void ShowEmployeeMenu(string employeeName)
    {
        Console.WriteLine($"\n--- Меню сотрудника {employeeName} ---");
        Console.WriteLine("1. Мои смены");
        Console.WriteLine("2. Запросить отпуск");
        Console.WriteLine("3. Отметиться на смену");
        Console.WriteLine("0. Выход");
        Console.Write("Выберите действие: ");
    }

    static void ShowAllEmployees()
    {
        Console.WriteLine("\n=== Список сотрудников ===");
        var employees = manager.Employees.ToList();
        if (employees.Count == 0) { Console.WriteLine("Нет сотрудников."); return; }
        for (int i = 0; i < employees.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {employees[i]}");
        }
    }

    static Employee SelectEmployee(string prompt)
    {
        var employees = manager.Employees.ToList();
        if (employees.Count == 0) { Console.WriteLine("Нет сотрудников."); return null; }

        Console.WriteLine("\n" + prompt);
        for (int i = 0; i < employees.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {employees[i].FullName} | {employees[i].Phone.Value}");
        }

        Console.Write("Введите номер сотрудника: ");
        if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= employees.Count)
        {
            return employees[index - 1];
        }
        Console.WriteLine("Неверный номер.");
        return null;
    }

    static void AddEmployee()
    {
        Console.Write("Введите ФИО сотрудника: ");
        string fullName = Console.ReadLine();
        Console.Write("Введите телефон сотрудника: ");
        string phoneStr = Console.ReadLine();

        try
        {
            var phone = new Phone(phoneStr);
            manager.CreateEmployee(fullName, phone);
            DataService.Save(manager);
            Console.WriteLine($"Сотрудник {fullName} добавлен.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void AssignShiftToEmployee()
    {
        var employee = SelectEmployee("Выберите сотрудника для назначения смены:");
        if (employee == null) return;

        Console.Write("Введите дату смены (гггг-мм-дд): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime shiftDate))
        {
            Console.WriteLine("Неверный формат даты.");
            return;
        }

        Console.Write("Введите количество часов: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal hours) || hours <= 0 || hours > 24)
        {
            Console.WriteLine("Часы должны быть от 1 до 24.");
            return;
        }

        try
        {
            manager.AssignShift(employee, shiftDate, hours);
            DataService.Save(manager);
            Console.WriteLine($"Смена на {shiftDate.ToShortDateString()} ({hours}ч) назначена {employee.FullName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void RemoveShiftFromEmployee()
    {
        var employee = SelectEmployee("Выберите сотрудника для удаления смены:");
        if (employee == null) return;

        Console.Write("Введите дату смены для удаления (гггг-мм-дд): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime shiftDate))
        {
            Console.WriteLine("Неверный формат даты.");
            return;
        }

        try
        {
            manager.RemoveShift(employee, shiftDate);
            DataService.Save(manager);
            Console.WriteLine($"Смена на {shiftDate.ToShortDateString()} удалена у {employee.FullName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void ShowVacationRequests()
    {
        var requests = manager.GetVacationRequests().ToList();
        Console.WriteLine("\n=== Заявки на отпуск ===");
        if (requests.Count == 0) { Console.WriteLine("Нет заявок."); return; }

        for (int i = 0; i < requests.Count; i++)
        {
            var emp = requests[i];
            string status = emp.VacationStatus switch
            {
                VacationStatus.Pending => "ожидает",
                VacationStatus.Approved => "одобрен",
                VacationStatus.Rejected => "отклонён",
                _ => emp.VacationStatus.ToString()
            };
            Console.WriteLine($"{i + 1}. {emp.FullName} | {emp.Phone.Value} | Статус: {status}");
        }
    }

    static void ApproveVacation()
    {
        var requests = manager.GetVacationRequests().ToList();
        if (requests.Count == 0) { Console.WriteLine("Нет заявок на отпуск."); return; }

        ShowVacationRequests();
        Console.Write("Введите номер заявки для одобрения: ");
        if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > requests.Count)
        {
            Console.WriteLine("Неверный номер.");
            return;
        }

        var employee = requests[index - 1];
        try
        {
            manager.ApproveVacation(employee);
            DataService.Save(manager);
            Console.WriteLine($"Отпуск {employee.FullName} утверждён.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void RejectVacation()
    {
        var requests = manager.GetVacationRequests().ToList();
        if (requests.Count == 0) { Console.WriteLine("Нет заявок на отпуск."); return; }

        ShowVacationRequests();
        Console.Write("Введите номер заявки для отклонения: ");
        if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > requests.Count)
        {
            Console.WriteLine("Неверный номер.");
            return;
        }

        var employee = requests[index - 1];
        try
        {
            manager.RejectVacation(employee);
            DataService.Save(manager);
            Console.WriteLine($"Отпуск {employee.FullName} отклонён.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void ShowMyShifts(Employee employee)
    {
        Console.WriteLine($"\n=== Смены сотрудника {employee.FullName} ===");
        var shifts = employee.Shifts.ToList();
        if (shifts.Count == 0) { Console.WriteLine("Нет смен."); return; }
        for (int i = 0; i < shifts.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {shifts[i]}");
        }
    }

    static void RequestMyVacation(Employee employee)
    {
        try
        {
            employee.RequestVacation();
            DataService.Save(manager);
            Console.WriteLine("Запрос на отпуск отправлен.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void MarkMyShift(Employee employee)
    {
        var shifts = employee.Shifts.ToList();
        if (shifts.Count == 0) { Console.WriteLine("У вас нет назначенных смен."); return; }

        Console.WriteLine("\nВыберите смену для отметки:");
        for (int i = 0; i < shifts.Count; i++)
        {
            string status = shifts[i].IsDone ? " (уже отмечена)" : "";
            Console.WriteLine($"{i + 1}. {shifts[i]}{status}");
        }

        Console.Write("Введите номер смены: ");
        if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > shifts.Count)
        {
            Console.WriteLine("Неверный номер.");
            return;
        }

        var selectedShift = shifts[index - 1];
        if (selectedShift.IsDone)
        {
            Console.WriteLine("Эта смена уже отмечена.");
            return;
        }

        try
        {
            manager.MarkShiftDone(employee, selectedShift.ShiftDate);
            DataService.Save(manager);
            Console.WriteLine($"Вы отметились на смену {selectedShift.ShiftDate.ToShortDateString()}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}