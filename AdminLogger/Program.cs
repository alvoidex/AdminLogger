using System.ComponentModel.Design;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Channels;

using static System.Runtime.InteropServices.JavaScript.JSType;
namespace ConsoleApp1
{
    internal class Program
    {
        enum MessageType
        {
            Path,
            Filter,
            Result,
            FullPath
        }
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
                Messages(MessageType.FullPath);
            }
        }
        static void PrintResults(List<string> Lines)
        {
            Messages(MessageType.Result);
            foreach (var item in Lines)
            {
                Console.WriteLine(item);
            }
        }
        static (List<string>, string Filter)? ReadLog(string path)
        {
            Messages(MessageType.Filter);
            string? Filter = Console.ReadLine()?.Trim().ToLower();
            if (Filter == "0") return null;
            Filter = string.IsNullOrWhiteSpace(Filter) ? "all" : Filter;
            var lines = File.ReadAllLines(path);
            var Filtered = lines.Where(line => Filter == "all" || line.ToLower().Contains(Filter)).ToList();
            return (Filtered, Filter);
        }
        static string? GetFilePath()
        {
            Messages(MessageType.Path);
            string? path = Console.ReadLine()?.Trim().Trim('"');
            return string.IsNullOrWhiteSpace(path) || !File.Exists(path) ? null : path;
        }
        static void Messages(MessageType type, string? rep = "rep_log.txt")
        {
            switch (type)
            {
                case MessageType.Path:
                    Console.WriteLine("1 | Введите корректный путь к файлу или перетащите файл в окно\n" + new string('═', 62));
                    break;
                case MessageType.Filter:
                    Console.WriteLine(new string('═', 79) + "\n2 | Введите любое слово для фильтра (пример: error, warn или 0 для нового файла)");
                    break;
                case MessageType.Result:
                    Console.WriteLine(new string('═', 14) + "\n3 | РЕЗУЛЬТАТЫ:");
                    break;
                case MessageType.FullPath:
                    Console.WriteLine($"4 | Отчет сохранен в rep_log.txt в папке с программой:\n{Path.GetFullPath(rep)}\n" + new string(':', 79));
                    break;
                default:
                    Console.WriteLine();
                    break;
            }
        }
    }
}
