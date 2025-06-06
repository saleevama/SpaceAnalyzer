using SpaceAnalyzer.Models;

//возвращает асинхронную операцию, результат работы - лист объектов FileData (основная модель)

namespace SpaceAnalyzer.Services
{
   public interface IScannerDirectory
    {
        Task<List<FileData>> ScannerAsync(string folder);
    }
}
