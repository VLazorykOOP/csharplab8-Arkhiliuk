using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

class Program
{
    static void Main()
    {
        string basePath = "/Users/admin/RiderProjects/lab8/lab8";
        
        // Завдання 1: Знайдіть IP адреси
        ProcessIPAddresses(Path.Combine(basePath, "input.txt"), Path.Combine(basePath, "outputIP.txt"));

        // Завдання 2: Вивести слова заданої довжини
        ExtractWordsByLength(Path.Combine(basePath, "input.txt"), Path.Combine(basePath, "outputWords.txt"), 5);

        // Завдання 3: Вилучити слова найбільшої довжини
        RemoveLongestWords(Path.Combine(basePath, "input.txt"), Path.Combine(basePath, "outputNoLongestWords.txt"));

        // Завдання 4: Записати зворотні числа і вивести компоненти файлу
        WriteAndReadInverseNumbers(Path.Combine(basePath, "numbers.bin"), 10);

        // Завдання 5: Робота з файлами і папками
        ManageDirectoriesAndFiles(Path.Combine(basePath, "temp"), "Ivanov");
    }

    static void ProcessIPAddresses(string inputPath, string outputPath)
    {
        string pattern = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";
        int count = 0;

        using (StreamReader reader = new StreamReader(inputPath))
        using (StreamWriter writer = new StreamWriter(outputPath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                foreach (Match match in Regex.Matches(line, pattern))
                {
                    writer.WriteLine(match.Value);
                    count++;
                }
            }
        }

        Console.WriteLine($"Total IP addresses found: {count}");
    }

    static void ExtractWordsByLength(string inputPath, string outputPath, int length)
    {
        using (StreamReader reader = new StreamReader(inputPath))
        using (StreamWriter writer = new StreamWriter(outputPath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var words = line.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                                .Where(word => word.Length == length);
                foreach (var word in words)
                {
                    writer.WriteLine(word);
                }
            }
        }
    }

    static void RemoveLongestWords(string inputPath, string outputPath)
    {
        using (StreamReader reader = new StreamReader(inputPath))
        using (StreamWriter writer = new StreamWriter(outputPath))
        {
            string text = reader.ReadToEnd();
            var words = text.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
            int maxLength = words.Max(w => w.Length);
            var filteredWords = words.Where(word => word.Length != maxLength);
            writer.WriteLine(String.Join(" ", filteredWords));
        }
    }

    static void WriteAndReadInverseNumbers(string filePath, int n)
    {
        using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
        {
            for (int i = 1; i <= n; i++)
            {
                double inverse = 1.0 / i;
                writer.Write(inverse);
            }
        }

        using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
        {
            int index = 0;
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                double value = reader.ReadDouble();
                index++;
                if (index % 3 == 0)  // Check if the index is a multiple of 3
                {
                    Console.WriteLine($"1/{index} = {value:F6}");  // Format the output to show fractions
                }
            }
        }
    }

    static void ManageDirectoriesAndFiles(string baseFolder, string studentSurname)
    {
        string studentFolder1 = Path.Combine(baseFolder, $"{studentSurname}1");
        string studentFolder2 = Path.Combine(baseFolder, $"{studentSurname}2");

        Directory.CreateDirectory(studentFolder1);
        Directory.CreateDirectory(studentFolder2);

        string file1Path = Path.Combine(studentFolder1, "t1.txt");
        string file2Path = Path.Combine(studentFolder1, "t2.txt");
        string file3Path = Path.Combine(studentFolder2, "t3.txt");

        File.WriteAllText(file1Path, "Шевченко Степан Іванович, 2001 року народження, місце проживання м. Суми");
        File.WriteAllText(file2Path, "Комар Сергій Федорович, 2000 року народження, місце проживання м. Київ");

        string text1 = File.ReadAllText(file1Path);
        string text2 = File.ReadAllText(file2Path);
        File.WriteAllText(file3Path, text1 + Environment.NewLine + text2);

        File.Move(file2Path, Path.Combine(studentFolder2, "t2.txt"));
        File.Copy(file1Path, Path.Combine(studentFolder2, "t1.txt"), true);

        Directory.Move(studentFolder2, Path.Combine(baseFolder, "ALL"));
        Directory.Delete(studentFolder1, true);

        foreach (var file in Directory.GetFiles(Path.Combine(baseFolder, "ALL")))
        {
            FileInfo fileInfo = new FileInfo(file);
            Console.WriteLine($"File: {fileInfo.Name}, Size: {fileInfo.Length} bytes");
        }
    }
}