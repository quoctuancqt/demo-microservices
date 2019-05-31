namespace CFileManager.Services
{
    using Microsoft.AspNetCore.Hosting;
    using System.IO;
    using System.Threading.Tasks;

    public class FileManager : IFileManager
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly string _exportFolder;

        public FileManager(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _exportFolder = $"{_hostingEnvironment.WebRootPath}\\tmp";
            if (!Directory.Exists(_exportFolder))
            {
                Directory.CreateDirectory(_exportFolder);
            }
        }

        public string BuildExportFilePath(string fileName)
        {
            return $"{_exportFolder}\\{fileName}";
        }

        public void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public async Task<byte[]> ReadFileAsync(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            {
                byte[] bytes = new byte[stream.Length];
                int numBytesToRead = (int)stream.Length;
                int numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    int n = await stream.ReadAsync(bytes, numBytesRead, numBytesToRead);

                    if (n == 0)
                        break;

                    numBytesRead += n;
                    numBytesToRead -= n;
                }

                return bytes;
            }
        }

        public FileInfo PrepareExportFile(string fileName)
        {
            string filePath = BuildExportFilePath(fileName);

            var file = new FileInfo(filePath);
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(filePath);
            }

            return file;
        }
    }
}
