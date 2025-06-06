using SpaceAnalyzer.Models;

namespace SpaceAnalyzer.Services
{
    public class ScannerDirectory : IScannerDirectory
    {
        public async Task<List<FileData>> ScannerAsync(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder))
                throw new ArgumentException("Путь не может быть пустым");

            if (!Directory.Exists(folder))
                throw new DirectoryNotFoundException("Несуществующая папка");

            var files = new List<FileData>();

            try
            {
                await Task.Run(() => ScanDirectory(folder, files));
            }
            catch (Exception ex)
            { 
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
              
                return files;
        }

        private void ScanDirectory(string currentDirectory, List<FileData> files)
        {
            try
            {
                foreach (string pathToFile in Directory.GetFiles(currentDirectory))
                {
                    try
                    {
                        var fileInfo = new FileInfo(pathToFile);
                        files.Add(new FileData(
                                pathToFile,
                                fileInfo.Extension,
                                fileInfo.Length,
                                fileInfo.CreationTime
                            ));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка обработки файлов {pathToFile}: {ex.Message}");
                    }
                }
                foreach (string subDirectory in Directory.GetDirectories(currentDirectory))
                {
                    ScanDirectory(subDirectory, files);
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine($"Нет доступа к {currentDirectory}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сканировании {currentDirectory}: {ex.Message}");
            }
        }
    }
}
