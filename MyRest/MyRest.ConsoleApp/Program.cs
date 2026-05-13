using System;
using MyRest.Domain.Entities;
using MyRest.Domain.ValueObjects;
using MyRest.Domain.Exceptions;
using MyRest.Domain.ValueObjects.Exceptions;

namespace MyRest.ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Тест доменного слоя");

        try
        {
            // 1. Тестируем валидацию имен (Value Objects)
            Console.WriteLine("\n[Тест 1]: Проверка слишком короткого имени");
            try
            {
                var tooShortName = new PersonName("А");
            }
            catch (ArgumentShortValueException ex)
            {
                Console.WriteLine("Успешно поймали ошибку: " + ex.Message);
            }

            // 2. Создаем нормальные объекты
            Console.WriteLine("\n[Тест 2]: Создание менеджеров и сотрудников");

            var managerIvan = new Manager(Guid.NewGuid(), new PersonName("Иван"), new PersonName("Иванов"));
            var managerPetr = new Manager(Guid.NewGuid(), new PersonName("Петр"), new PersonName("Петров"));

            // Иван нанимает Анну
            var empAnna = managerIvan.HireEmployee(
                new PersonName("Анна"),
                new PersonName("Смирнова"),
                new PhoneNumber("+79001112233"));

            // Петр нанимает Олега
            var empOleg = managerPetr.HireEmployee(
                new PersonName("Олег"),
                new PersonName("Олегов"),
                new PhoneNumber("+79004445566"));

            Console.WriteLine($"Менеджер {managerIvan.FirstName.Value} нанял сотрудника {empAnna.FirstName.Value}");
            Console.WriteLine($"Менеджер {managerPetr.FirstName.Value} нанял сотрудника {empOleg.FirstName.Value}");

            // 3. Тестируем бизнес-правило "Свой/Чужой" (SOLID)
            Console.WriteLine("\n[Тест 3]: Иван пытается уволить сотрудника Петра");
            try
            {
                // Иван не может уволить Олега, так как Олег принадлежит Петру
                managerIvan.FireEmployee(empOleg);
            }
            catch (EmployeeNotBelongManagerException ex)
            {
                Console.WriteLine("Ошибка доступа подтверждена: " + ex.Message);
            }

            // 4. Тестируем легальное изменение состояния
            Console.WriteLine("\n[Тест 4]: Петр увольняет своего сотрудника");
            bool isFired = managerPetr.FireEmployee(empOleg);

            if (isFired)
            {
                Console.WriteLine($"Сотрудник {empOleg.FirstName.Value} теперь имеет статус: {empOleg.Status}");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("\n!!! Непредвиденная ошибка: " + ex.Message);
        }

        Console.WriteLine("\nТестирование завершено. Нажмите Enter...");
        Console.ReadLine();
    }
}