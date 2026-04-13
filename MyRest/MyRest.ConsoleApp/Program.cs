using System;
using MyRest.Domain.Entities;
using MyRest.Domain.ValueObjects;
using MyRest.Domain.Exceptions;
using VOExceptions = MyRest.Domain.ValueObjects.Exceptions;

namespace MyRest.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ТЕСТИРОВАНИЕ БИЗНЕС-ПРАВИЛ MY REST\n");

            try
            {
                // БЛОК 1: ИНИЦИАЛИЗАЦИЯ (УСПЕШНЫЙ СЦЕНАРИЙ)

                Console.WriteLine(">>> БЛОК 1: Создание базы (2 менеджера, 2 сотрудника)...");

                var managerIvan = new Manager(new PersonName("Иван"), new PersonName("Иванов"), new PhoneNumber("+79001111111"));
                var empAnna = managerIvan.HireEmployee(new PersonName("Анна"), new PersonName("Смирнова"), new PhoneNumber("+79002222222"));

                var managerPetr = new Manager(new PersonName("Петр"), new PersonName("Петров"), new PhoneNumber("+79003333333"));
                var empOleg = managerPetr.HireEmployee(new PersonName("Олег"), new PersonName("Олегов"), new PhoneNumber("+79004444444"));

                Console.WriteLine("Сотрудники успешно наняты своими менеджерами.\n");

               
                // БЛОК 2: ПРОВЕРКА ПРАВ ДОСТУПА (СВОЙ/ЧУЖОЙ)
                
                Console.WriteLine(">>> БЛОК 2: Тестирование изоляции подразделений...");

                TestRule("Менеджер Иван пытается уволить чужого сотрудника (Олега)", () =>
                {
                    managerIvan.FireEmployee(empOleg);
                });

                TestRule("Менеджер Иван пытается назначить чужого сотрудника на смену", () =>
                {
                    var shift = managerIvan.CreateShift(DateOnly.FromDateTime(DateTime.Now), new TimeOnly(9, 0), 8m);
                    managerIvan.AssignShift(empOleg, shift);
                });

                TestRule("Менеджер Иван пытается одобрить отпуск чужому сотруднику", () =>
                {
                    var vac = empOleg.RequestVacation(new DateOnly(2024, 6, 1), new DateOnly(2024, 6, 14));
                    managerIvan.ResolveVacation(empOleg, vac, true);
                });

                TestRule("Сотрудник Анна пытается отметиться на чужой смене", () =>
                {
                    var shift = managerPetr.CreateShift(DateOnly.FromDateTime(DateTime.Now), new TimeOnly(10, 0), 8m);
                    var assignmentOleg = managerPetr.AssignShift(empOleg, shift);
                    // Анна пытается закрыть смену Олега
                    empAnna.MarkAttendance(assignmentOleg);
                });

                
                // БЛОК 3: ПРОВЕРКА БИЗНЕС-ЛОГИКИ
                
                Console.WriteLine(">>> БЛОК 3: Тестирование логики объектов...");

                TestRule("Менеджер создает смену длительностью 13 часов (максимум 12)", () =>
                {
                    managerIvan.CreateShift(DateOnly.FromDateTime(DateTime.Now), new TimeOnly(9, 0), 13m);
                });

                TestRule("Сотрудник запрашивает отпуск в обратную сторону (конец раньше начала)", () =>
                {
                    empAnna.RequestVacation(new DateOnly(2024, 8, 10), new DateOnly(2024, 8, 1));
                });

                TestRule("Сотрудник запрашивает отпуск раньше чем через 6 месяцев", () =>
                {
                    // Успешный отпуск в мае
                    var vac1 = empAnna.RequestVacation(new DateOnly(2024, 5, 1), new DateOnly(2024, 5, 14));
                    managerIvan.ResolveVacation(empAnna, vac1, true);

                    // Попытка взять отпуск в августе
                    empAnna.RequestVacation(new DateOnly(2024, 8, 1), new DateOnly(2024, 8, 14));
                });

                // БЛОК 4: ПРОВЕРКА СОСТОЯНИЙ (СТАТУС УВОЛЕННОГО)
                
                Console.WriteLine(">>> БЛОК 4: Тестирование жизненного цикла (увольнение)...");

                // Увольняем Олега легально
                managerPetr.FireEmployee(empOleg);
                Console.WriteLine("[-] Менеджер Петр уволил Олега.");

                TestRule("Менеджер Петр пытается уволить Олега второй раз", () =>
                {
                    managerPetr.FireEmployee(empOleg);
                });

                TestRule("Уволенный Олег пытается запросить отпуск", () =>
                {
                    empOleg.RequestVacation(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 10));
                });

                TestRule("Менеджер Петр пытается назначить уволенного Олега на смену", () =>
                {
                    var shift = managerPetr.CreateShift(DateOnly.FromDateTime(DateTime.Now), new TimeOnly(9, 0), 8m);
                    managerPetr.AssignShift(empOleg, shift);
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[КРИТИЧЕСКАЯ СИСТЕМНАЯ ОШИБКА]: {ex.Message}");
            }

            
            // БЛОК 5: ПРОВЕРКА ВАЛИДАТОРОВ (VALUE OBJECTS)
            
            Console.WriteLine(">>> БЛОК 5: Тестирование входных данных...");

            TestValidationRule("Попытка нанять сотрудника с цифрами в имени (Олег123)", () =>
            {
                var badName = new PersonName("Олег123");
            });

            Console.WriteLine("\nТЕСТИРОВАНИЕ УСПЕШНО ЗАВЕРШЕНО");
            Console.ReadLine();
        }

        // Вспомогательный метод TestRule
        static void TestRule(string description, Action action)
        {
            Console.WriteLine($"- ТЕСТ: {description}");
            try
            {
                action.Invoke();
                Console.WriteLine("  [ПРОБЛЕМА]: Ошибка не сработала! Правило нарушено.");
            }
            catch (InvalidEntityStateException ex)
            {
                Console.WriteLine($"  [ОЖИДАЕМЫЙ ОТКАЗ]: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  [НЕОЖИДАННАЯ ОШИБКА]: {ex.Message}");
            }
            Console.WriteLine();
        }

        // Вспомогательный метод для перехвата ошибок валидации
        static void TestValidationRule(string description, Action action)
        {
            Console.WriteLine($"- ТЕСТ: {description}");
            try
            {
                action.Invoke();
                Console.WriteLine("  [ПРОБЛЕМА]: Ошибка не сработала! Валидатор пропустил плохие данные.");
            }
            catch (VOExceptions.DomainException ex)
            {
                Console.WriteLine($"  [ОЖИДАЕМЫЙ ОТКАЗ]: {ex.Message}");
            }
            Console.WriteLine();
        }
    }
}