using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Channels;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace ConsoleApp1
{
    internal class Program
    {
        static void Main()
        {
            string? path = null;
            while (path == null)
            {
                path = GetFilePath();
            }
                ViewLog(path);
        }
        static void ViewLog(string path)
        {
            List<string> filtered = ReadLog(path);
            Console.WriteLine("Результаты:");
            foreach (var item in filtered)
            {
                Console.WriteLine(item);
            }

            File.WriteAllLines("report.txt", filtered);
            Console.WriteLine("Отчет сохранен в report.txt в папке с программой");
        }
        static List<string> ReadLog(string path)
        {
            Console.WriteLine("Введите любое слово для фильтра (error, warn, info или all");
            string Filter = Console.ReadLine().Trim().ToLower();
            Filter = string.IsNullOrWhiteSpace(Filter) ? "all" : Filter;
            var lines = File.ReadAllLines(path);
            var filtered = lines.Where(line => Filter == "all" || line.ToLower().Contains(Filter)).ToList();
            return filtered;
        }
        static string? GetFilePath()
        {
            Console.WriteLine("Введите корректный путь к файлу");
            string path = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                return null;
            }
            return path;
        }
    }
}
