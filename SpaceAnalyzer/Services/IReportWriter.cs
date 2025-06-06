using SpaceAnalyzer.Models;

namespace SpaceAnalyzer.Services
{
    public interface IReportWriter
    {
        Task WriteReportAsync(List<FileData> files, string outputPath);
    }
}
