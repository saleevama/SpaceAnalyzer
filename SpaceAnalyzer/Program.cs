using SpaceAnalyzer.Models;
using SpaceAnalyzer.Services;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Анализатор дискового пространства ===");
        Console.WriteLine();

        try
        {
            // 1. Получаем путь для сканирования
            string folderPath = args.Length > 0 ? args[0] : GetFolderPathFromUser();

            // 2. Сканируем директорию
            var scanner = new ScannerDirectory();
            List<FileData> files = await scanner.ScannerAsync(folderPath);

            // 3. Генерируем отчет
            if (files.Any())
            {
                var reporter = new ReportWriter();
                string reportPath = Path.Combine(folderPath, "disk_report.csv");
                await reporter.WriteReportAsync(files, reportPath);

                // 4. Показываем статистику
                ShowStatistics(files);
            }
            else
            {
                Console.WriteLine("В указанной директории не найдено файлов.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⛔ Ошибка: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }

    private static string GetFolderPathFromUser()
    {
        string folderPath;
        do
        {
            Console.Write("Введите путь к директории для сканирования: ");
            folderPath = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(folderPath))
            {
                Console.WriteLine("Путь не может быть пустым!");
            }
        } while (string.IsNullOrWhiteSpace(folderPath));

        return folderPath;
    }

    private static void ShowStatistics(List<FileData> files)
    {
        Console.WriteLine("\nСтатистика:");
        Console.WriteLine($"Всего файлов: {files.Count}");

        var totalSize = files.Sum(f => f.GetSize());
        Console.WriteLine($"Общий размер: {FormatSize(totalSize)}");

        // Топ 10 самых больших файлов
        Console.WriteLine("\nТоп 10 самых больших файлов:");
        var topFiles = files
            .OrderByDescending(f => f.GetSize())
            .Take(10)
            .ToList();

        foreach (var file in topFiles)
        {
            Console.WriteLine($"{FormatSize(file.GetSize()),-10} | {file.GetPath()}");
        }
    }

    private static string FormatSize(double sizeInBytes)
    {
        string[] units = { "bytes", "KB", "MB", "GB" };
        double size = sizeInBytes;
        int unitIndex = 0;

        while (size >= 1024 && unitIndex < units.Length - 1)
        {
            size /= 1024;
            unitIndex++;
        }

        return $"{Math.Round(size, 2)} {units[unitIndex]}";
    }
}