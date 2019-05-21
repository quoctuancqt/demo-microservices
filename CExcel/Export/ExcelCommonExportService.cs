namespace CExcel.Export
{
    using CExcel.Attributes;
    using CFileManager.Services;
    using Common.Helpers;
    using OfficeOpenXml;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class ExcelCommonExportService : IExcelCommonExportService
    {
        private readonly IFileManager _fileManager;
        private readonly IExportHeaderGetter _exportHeaderGetter;

        public ExcelCommonExportService(
            IFileManager fileManager,
            IExportHeaderGetter exportHeaderGetter)
        {
            _fileManager = fileManager;
            _exportHeaderGetter = exportHeaderGetter;
        }

        public string Export<T>(string fileName, T[] models) where T : class
        {
            FileInfo file = _fileManager.PrepareExportFile(fileName);

            using (var package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                Dictionary<string, ExportInfoAttribute> exportHeaders = _exportHeaderGetter.Get<T>();

                // First add the headers
                for (var i = 0; i < exportHeaders.Values.Count; i++)
                {
                    worksheet.Cells[1, (i + 1)].Value = exportHeaders.Values.ElementAt(i).Header;
                }

                // Style headers
                worksheet.Cells[1, exportHeaders.Count + 1].Style.Font.Bold = true;

                // Add values
                for (var row = 0; row < models.Length; row++)
                {
                    for (var col = 0; col < exportHeaders.Count; col++)
                    {
                        worksheet.Cells[row + 2, col + 1].Value = models[row].GetPropValue(exportHeaders.Keys.ElementAt(col));
                    }
                }

                package.Save();
            }

            return _fileManager.BuildExportFilePath(fileName);
        }
    }
}
