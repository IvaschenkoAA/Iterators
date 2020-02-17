using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace IteratorsApp
{
    class Program
    {
        private static void Main()
        {
            string path = "..\\..\\Program.cs";
            if (!File.Exists(path))
                throw new FileNotFoundException($"Не найден файл по пути \"{path}\"");

            IEnumerable<string> lines1 = ReadFileUsingYieldReturn(path);
            IEnumerable<string> lines2 = ReadFileUsingReturn(path);
            IEnumerable<string> lines3 = ReadFileUsingLinq(path);

            PrintFileContents($"Вывод строк из файла {path} через yield return:", lines1);
            PrintFileContents($"Вывод строк из файла {path} через return:", lines2);
            PrintFileContents($"Вывод строк из файла {path} через LINQ:", lines3);

            Console.ReadLine();
        }

        private static IEnumerable<string> ReadFileUsingYieldReturn(string path)
        {
            using (var streamReader = new StreamReader(path))
            {
                foreach (string line in ReadLinesFromStream(streamReader))
                    yield return line;
            }
        }

        private static IEnumerable<string> ReadFileUsingReturn(string path)
        {
            using (var streamReader = new StreamReader(path))
            {
                return ReadLinesFromStream(streamReader);
            }
        }

        private static IEnumerable<string> ReadFileUsingLinq(string path)
        {
            using (var streamReader = new StreamReader(path))
            {
                return ReadLinesFromStream(streamReader).Select(l => l);
            }
        }

        private static IEnumerable<string> ReadLinesFromStream(StreamReader streamReader)
        {
            string line;
            while ((line = streamReader.ReadLine()) != null)
                yield return line;
        }

        private static void PrintFileContents(string headerMessage, IEnumerable<string> lines)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(headerMessage);
                Console.ForegroundColor = ConsoleColor.White;

                foreach (string line in lines)
                    Console.WriteLine(line);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Чтение файла завершилось с ошибкой: {e.Message}");
            }

            Console.WriteLine();
        }
    }
}
