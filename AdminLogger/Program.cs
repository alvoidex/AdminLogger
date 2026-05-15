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
            Intro,
            SrcPath,
            FilterWord,
            OutLog,
            ResltPath
        }
        const string OutLog = "rep_log.txt";
        static readonly string decorline = new string('─', 64);
        static void Main()
        {
            while (true)
            {
                Messages(MessageType.Intro);
                string? src_path = GetFilePath();
                if (src_path == null) continue;

                ViewLog(src_path);
            }
        }
        static void ViewLog(string src_path)
        {
            while (true)
            {
                var result = ReadLog(src_path);
                if (result == null) return;

                var (filtered_lines, filter_word) = result.Value;

                PrintResults(filtered_lines);
                string status = BuildStatus(filter_word, filtered_lines, src_path);
                filtered_lines.Insert(0, $"🔎 {status}\n");

                File.WriteAllLines(OutLog, filtered_lines);
                Console.WriteLine(decorline + $"\n -> {status}\n" + decorline);
                Messages(MessageType.ResltPath);
            }
        }
        static string BuildStatus(string filter_word, List<string> filtered_lines, string src_path)
        {
            return $"Поиск: {filter_word} | Найдено: {filtered_lines.Count} строк | Время {DateTime.Now:T} | Исходный файл: {src_path}";
            ;
        }
        static void PrintResults(List<string> lines)
        {
            Messages(MessageType.OutLog);
            foreach (var item in lines)
            {
                Console.WriteLine(item);
            }
        }
        static (List<string>, string Filter)? ReadLog(string src_path)
        {
            Messages(MessageType.FilterWord);
            var raw_lines = File.ReadAllLines(src_path);
            string? filter_word = Console.ReadLine()?.Trim().ToLower();
            if (filter_word == "0")
            { Console.Clear(); return null; }
            filter_word = string.IsNullOrWhiteSpace(filter_word) ? "all" : filter_word;
            var filtered_lines = raw_lines.Where(line => filter_word == "all" || line.ToLower().Contains(filter_word)).ToList();
            return (filtered_lines, filter_word);
        }
        static string? GetFilePath()
        {
            Messages(MessageType.SrcPath);
            string? src_path = Console.ReadLine()?.Trim().Trim('"');
            return string.IsNullOrWhiteSpace(src_path) || !File.Exists(src_path) ? null : src_path;
        }
        static void Messages(MessageType type, string? rep = OutLog)
        {
            switch (type)
            {
                case MessageType.Intro:
                    Console.WriteLine("┌──────────────────────── ADMIN LOGGER ────────────────────────┐");
                    break;
                case MessageType.SrcPath:
                    Console.WriteLine("1 | Введите корректный путь к файлу или перетащите файл в окно\n" + decorline);
                   break;
                case MessageType.FilterWord:
                    Console.WriteLine(decorline + "\n2 | Введите любое слово для фильтра (пример: error, warn или 0 для нового файла)");
                    break;
                case MessageType.OutLog:
                    Console.WriteLine(decorline + "\n3 | РЕЗУЛЬТАТЫ:");
                    break;
                case MessageType.ResltPath:
                    Console.WriteLine($"4 | Отчет сохранен в {OutLog} в папке с программой:\n{Path.GetFullPath(rep)}");
                    break;
                default:
                    Console.WriteLine();
                    break;
            }
        }
    }
}
