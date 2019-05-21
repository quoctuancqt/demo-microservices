namespace CExcel.Services
{
    using System.Collections.Generic;
    using System.IO;

    public interface IExcelService
    {
        IList<KeyValuePair<string, object>> ReadFile(string path);

        IList<T> ReadFile<T>(string path) where T : class, new();

        IList<KeyValuePair<string, object>> ReadFile(Stream stream);

        IList<T> ReadFile<T>(Stream stream) where T : class, new();
    }
}
