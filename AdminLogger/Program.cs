using System.Linq;
namespace ConsoleApp1
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Введите путь к файлу");
            string path = Console.ReadLine();

            if (!File.Exists(path))
            {
                Console.WriteLine("Файл не найден");
                return;
            }

            Console.WriteLine("Введите слово для фильтра (error, warn, info или all)");
            string Filter = Console.ReadLine();

            var lines = File.ReadAllLines(path);
            var filtered = lines.Where(line => Filter == "all" || line.Contains(Filter)).ToList();

            Console.WriteLine("Результаты:");
            foreach (var item in lines)
            {
                Console.WriteLine(item);
            }

            File.WriteAllLines("report.txt", filtered);
            Console.WriteLine("\nОтчет сохранен в report.txt");
        }
    }
}
