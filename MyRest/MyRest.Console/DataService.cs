#nullable disable
using System.Text.Json;
using System.Text.Json.Serialization;
using MyRest.Domain.Entities;

namespace MyRest.App;

public static class DataService
{
    private static readonly string _filePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
        "RestaurantData.json"
    );

    public class SaveData
    {
        public Manager Manager { get; set; }
    }

    public static void Save(Manager manager)
    {
        try
        {
            var data = new SaveData { Manager = manager };
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                MaxDepth = 64,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(_filePath, json);
            Console.WriteLine("Данные сохранены.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сохранения: {ex.Message}");
        }
    }

    public static Manager Load()
    {
        if (!File.Exists(_filePath))
        {
            Console.WriteLine("Файл с данными не найден. Будет создан новый менеджер.");
            return null;
        }

        try
        {
            string json = File.ReadAllText(_filePath);
            var options = new JsonSerializerOptions
            {
                MaxDepth = 64,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
            var data = JsonSerializer.Deserialize<SaveData>(json, options);

            if (data?.Manager != null)
            {
                foreach (var emp in data.Manager.Employees)
                {
                    emp.Manager = data.Manager;
                }
                Console.WriteLine("Данные загружены из файла.");
            }
            return data?.Manager;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки: {ex.Message}");
            return null;
        }
    }
}