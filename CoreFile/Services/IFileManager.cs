namespace CFileManager.Services
{
    using System.IO;
    using System.Threading.Tasks;

    public interface IFileManager
    {
        string BuildExportFilePath(string fileName);

        FileInfo PrepareExportFile(string fileName);

        Task<byte[]> ReadFileAsync(string filePath);

        void DeleteFile(string filePath);
    }
}
