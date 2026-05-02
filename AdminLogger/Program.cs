using System.Linq;
using System.Threading.Channels;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace ConsoleApp1
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Введите корректный путь к файлу");
            bool get_path = Path_Checking(out string? path);
            string message = get_path == false ? "Введите корректный путь к файлу" :
                "Введите слово для фильтра (error, warn, info или all)";
            if (get_path)
            {
                Console.WriteLine(message);
                ViewLog(path);
            }
            else
            {
                Console.WriteLine(message);
            }
        }
        static void ViewLog(string path)
        {
            string Filter = Console.ReadLine().Trim().ToLower();

            var lines = File.ReadAllLines(path);
            var filtered = lines.Where(line => Filter == "all" || line.ToLower().Contains(Filter)).ToList();

            Console.WriteLine("Результаты:");
            foreach (var item in filtered)
            {
                Console.WriteLine(item);
            }

            File.WriteAllLines("report.txt", filtered);
            Console.WriteLine("\nОтчет сохранен в report.txt");
        }
        static bool Path_Checking(out string? path)
        {
            path = Console.ReadLine();
            if (!File.Exists(path))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
