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

                string? path = GetFilePath();
                if (path == null)
                {
                    continue;
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
                PrintResults(filtered);
                string status = $"Поиск: {Filter} | Найдено: {filtered.Count} строк | Время {DateTime.Now:T} | Исходный файл: {path}";
                filtered.Insert(0, $"🔎 {status}\n");
                File.WriteAllLines("rep_log.txt", filtered);
                Console.WriteLine(new string('-', 79) + $"\n -> {status}\n" + new string('-', 79));
                Console.WriteLine($"4 | Отчет сохранен в rep_log.txt в папке с программой:\n{Path.GetFullPath("rep_log.txt")}\n" + new string(':', 79));
            }
        }
        static void PrintResults(List<string> Lines)
        {
            Console.WriteLine(new string('═', 14) + "\n3 | РЕЗУЛЬТАТЫ:");
            foreach (var item in Lines)
            {
                Console.WriteLine(item);
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
            string? path = Console.ReadLine()?.Trim().Trim('"');
            return string.IsNullOrWhiteSpace(path) || !File.Exists(path) ? null : path;
        }
    }
}
