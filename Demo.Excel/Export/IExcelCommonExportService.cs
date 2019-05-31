namespace CExcel.Export
{
    public interface IExcelCommonExportService
    {
        string Export<T>(string fileName, T[] models) where T : class;
    }
}
