namespace CFileManager.Helpers
{
    using CFileManager.Enums;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;

    public class FileHelper
    {
        private const string END_POINT = "/get";

        public static void CreateFolder(ref string folder, string id, bool isDocument = false, string documentType = "")
        {
            try
            {
                if (!isDocument)
                {
                    folder = $"\\{id}\\{DateTime.Now.Year}-{DateTime.Now.Month}";
                }
                else
                {
                    folder = $"\\{documentType}";
                }

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
            }
            catch
            {
                folder = null;
            }
        }

        public static void GenerateFile(ref string diskPath, ref string url, string fileName)
        {
            diskPath = $"{diskPath}\\{fileName}";

            url = $"{END_POINT}?fileName={fileName}";
        }

        public static void DownLoadImage(string url, string path)
        {
            DeleteFile(path);

            if (!string.IsNullOrEmpty(url))
            {
                using (WebClient client = new WebClient())
                {
                    Task.Factory.StartNew(() => client.DownloadFileAsync(new Uri(url), path))
                         .ContinueWith((T) => { }, TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }

        public static void DeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static void SaveFile(ref string diskPath, ref string url, string fileName, string rootFolder, IFormFile file, bool isDocument = false, string documentType = "")
        {
            if (file == null) return;

            CreateFolder(ref diskPath, rootFolder, isDocument, documentType);

            GenerateFile(ref diskPath, ref url, fileName);

            using (var stream = new FileStream(diskPath, FileMode.Create))
            {
                file.CopyToAsync(stream).GetAwaiter().GetResult();
            }
        }

        public static bool CheckImageType(IFormFile file)
        {
            if (file == null) return true;

            string ext = Path.GetExtension(file.FileName);

            switch (ext.ToLower())
            {
                case ".gif":
                    return true;
                case ".jpg":
                    return true;
                case ".jpeg":
                    return true;
                case ".png":
                    return true;
                default:
                    return false;
            }
        }

        public static void WriteFile(string path, string txt)
        {
            List<string> lines = new List<string>() { txt };

            if (File.Exists(path))
            {
                lines.AddRange(File.ReadAllLines(path));
            }

            using (StreamWriter file = new StreamWriter(path))
            {
                foreach (string line in lines)
                {
                    file.WriteLine(line);
                }
            }
        }

        public static FileType MapFileExtensionToFileType(string fileExtension)
        {
            switch (fileExtension)
            {
                case "image/jpeg": return FileType.Image;
                case "application/msword": return FileType.Doc;
                default: return FileType.Pdf;
            }
        }

    }
}
