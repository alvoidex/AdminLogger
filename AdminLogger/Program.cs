using System.ComponentModel.Design;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Channels;

using static System.Runtime.InteropServices.JavaScript.JSType;
namespace ConsoleApp1
{
    internal class Program
    {
        static void Main()
        {
            while (true)
            {

                string? path = null;
                while (path == null)
                {
                    path = GetFilePath();
                }
                ViewLog(path);
            }
        }
        static void ViewLog(string path)
        {
            while (true)
            {
                List<string> filtered = ReadLog(path);
                if (filtered == null)
                    return;
                Console.WriteLine("Результаты:");
                foreach (var item in filtered)
                {
                    Console.WriteLine(item);
                }
                File.WriteAllLines("report.txt", filtered);
                Console.WriteLine("══════════════════════════════════════════════════\n" +
                "√ Отчет сохранен в report.txt в папке с программой");
            }
        }
        static List<string>? ReadLog(string path)
        {
            Console.WriteLine("═════════════════════════════════════════════════════════════════════════════\n► Введите любое слово для фильтра (пример: error, warn или 0 для нового файла)");
            string? Filter = Console.ReadLine()?.Trim().ToLower();
            if (Filter == "0")
                return null;
            Filter = string.IsNullOrWhiteSpace(Filter) ? "all" : Filter;
            var lines = File.ReadAllLines(path);
            return lines.Where(line => Filter == "all" || line.ToLower().Contains(Filter)).ToList();
        }
        static string? GetFilePath()
        {
            Console.WriteLine("> Введите корректный путь к файлу или перетащите файл в окно\n════════════════════════════════════════════════════════════");
            string path = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                return null;
            }
            return path;
        }
    }
}
