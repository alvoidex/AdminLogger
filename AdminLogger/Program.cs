using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
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
        const string RepFile = "rep_log.txt";
        static void Main()
        {
            Console.WriteLine(new string('═', 62));
            while (true)
            {
                string? path = GetFilePath();
                if (path == null) continue;

                ViewLog(path);
            }
        }
        static void ViewLog(string path)
        {
            while (true)
            {
                var result = ReadLog(path);
                if (result == null) return;

                var (filtered, filter_word) = result.Value;

                PrintResults(filtered);
                string status = BuildStatus(filter_word, filtered, path);
                filtered.Insert(0, $"🔎 {status}\n");

                File.WriteAllLines(RepFile, filtered);
                Console.WriteLine(new string(':', 79) + $"\n -> {status}\n" + new string(':', 79));
                Messages(MessageType.FullPath);
            }
        }
        static string BuildStatus(string filter, List<string> filtered, string path)
        {
            return $"Поиск: {filter} | Найдено: {filtered.Count} строк | Время {DateTime.Now:T} | Исходный файл: {path}";
        }
        static void PrintResults(List<string> lines)
        {
            Messages(MessageType.Result);
            foreach (var item in lines)
            {
                Console.WriteLine(item);
            }
        }
        static (List<string>, string Filter)? ReadLog(string path)
        {
            Messages(MessageType.Filter);
            var lines = File.ReadAllLines(path);
            string? filter_word = Console.ReadLine()?.Trim().ToLower();
            if (filter_word == "0") return null;
            filter_word = string.IsNullOrWhiteSpace(filter_word) ? "all" : filter_word;
            var Filtered = lines.Where(line => filter_word == "all" || line.ToLower().Contains(filter_word)).ToList();
            return (Filtered, filter_word);
        }
        static string? GetFilePath()
        {
            Messages(MessageType.Path);
            string? path = Console.ReadLine()?.Trim().Trim('"');
            return string.IsNullOrWhiteSpace(path) || !File.Exists(path) ? null : path;
        }
        static void Messages(MessageType type, string? rep = RepFile)
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
                    Console.WriteLine($"4 | Отчет сохранен в {RepFile} в папке с программой:\n{Path.GetFullPath(rep)}");
                    break;
                default:
                    Console.WriteLine();
                    break;
            }
        }
    }
}
