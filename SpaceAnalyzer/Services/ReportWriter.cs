using SpaceAnalyzer.Models;

namespace SpaceAnalyzer.Services
{
    public class ReportWriter : IReportWriter
    {
        public async Task WriteReportAsync(List<FileData> files, string outputPath)
        {
            ValidateInput(files, outputPath);

            try
            {
                await using var writer = new StreamWriter(outputPath);
                await WriteReportContentAsync(writer, files);
                Console.WriteLine($"Отчет сохранен: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка записи отчета: {ex.Message}");
                throw;
            }
        }

        private void ValidateInput(List<FileData> files, string outputPath)
        {
            if (files == null)
                throw new ArgumentNullException(nameof(files));

            if (string.IsNullOrWhiteSpace(outputPath))
                throw new ArgumentException("Путь пуст", nameof(outputPath));
        }

        private async Task WriteReportContentAsync(StreamWriter writer, List<FileData> files)
        {
            await writer.WriteLineAsync("Путь, Тип, Размер, Дата создания");

            foreach (var file in files)
            {
                await writer.WriteLineAsync(
                    $"\"{file.GetPath}\",{file.GetType},{file.GetSize},{file.GetCreatedDate:yyyy-MM-dd}");
            }
        }
    }
}