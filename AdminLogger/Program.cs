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
            Console.WriteLine(new string('═', 62));
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
                var result = ReadLog(path);
                if (result == null) return;
                var (filtered, Filter) = result.Value;
                //if (filtered == null) return;
                Console.WriteLine(new string('═', 14)+"\n3 | РЕЗУЛЬТАТЫ:");
                foreach (var item in filtered)
                {
                    Console.WriteLine(item);
                }
                File.WriteAllLines("report.txt", filtered);
                Console.WriteLine(new string('-', 79) + $"\n > Поиск: {Filter} | Строк найдено: {filtered.Count} | Время {DateTime.Now:T} | Файл: {path}".PadLeft(10));
                Console.WriteLine("\n4 | Отчет сохранен в report.txt в папке с программой");
            }
        }
        static (List<string>, string Filter)? ReadLog(string path)
        {
            Console.WriteLine(new string('═',79)+ "\n2 | Введите любое слово для фильтра (пример: error, warn или 0 для нового файла)");
            string? Filter = Console.ReadLine()?.Trim().ToLower();
            if (Filter == "0") return null;
            Filter = string.IsNullOrWhiteSpace(Filter) ? "all" : Filter;
            var lines = File.ReadAllLines(path);
            var Filtered = lines.Where(line => Filter == "all" || line.ToLower().Contains(Filter)).ToList();
            return (Filtered, Filter);
        }
        static string? GetFilePath()
        {
            Console.WriteLine("1 | Введите корректный путь к файлу или перетащите файл в окно\n"+ new string('═', 62));
            string path = Console.ReadLine();
            return string.IsNullOrWhiteSpace(path) || !File.Exists(path) ? null : path;
        }
    }
}
