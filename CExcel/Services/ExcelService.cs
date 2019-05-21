namespace CExcel.Services
{
    using CExcel.Exceptions;
    using CExcel.Import;
    using Common.Helpers;
    using OfficeOpenXml;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class ExcelService : IExcelService
    {
        private readonly IImportHeaderGetter _importHeaderGetter;

        public ExcelService(IImportHeaderGetter importHeaderGetter)
        {
            _importHeaderGetter = importHeaderGetter;
        }

        public virtual IList<KeyValuePair<string, object>> ReadFile(string path)
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                return GetTable(worksheet, GetHeader(worksheet));
            }
        }

        public virtual IList<T> ReadFile<T>(string path)
            where T : class, new()
        {

            using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                return GetTable<T>(worksheet);
            }

        }

        public IList<KeyValuePair<string, object>> ReadFile(Stream stream)
        {
            using (ExcelPackage package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                return GetTable(worksheet, GetHeader(worksheet));
            }
        }

        public IList<T> ReadFile<T>(Stream stream) where T : class, new()
        {
            using (ExcelPackage package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                return GetTable<T>(worksheet);
            }
        }

        private IList<string> GetHeader(ExcelWorksheet worksheet)
        {
            var headers = new List<string>();

            for (int col = 1; col <= worksheet.Dimension.Columns; col++)
            {
                headers.Add(worksheet.Cells[1, col].Value.ToString().Replace(" ", ""));
            }

            return headers;
        }

        private IList<KeyValuePair<string, object>> GetTable(ExcelWorksheet worksheet, IList<string> keys)
        {
            var table = new List<KeyValuePair<string, object>>();

            int rowCount = worksheet.Dimension.Rows;

            int ColCount = worksheet.Dimension.Columns;

            for (int row = 2; row <= rowCount; row++)
            {
                for (int col = 1; col <= ColCount; col++)
                {
                    var value = worksheet.Cells[row, col].GetValue<object>();

                    table.Add(new KeyValuePair<string, object>(keys[col - 1], value));
                }
            }

            return table;
        }

        private IList<T> GetTable<T>(ExcelWorksheet worksheet)
             where T : class, new()
        {
            try
            {
                var table = new List<T>();

                var properties = _importHeaderGetter.Get<T>();

                int rowCount = worksheet.Dimension.Rows;

                int ColCount = worksheet.Dimension.Columns <= properties.Length
                    ? worksheet.Dimension.Columns
                    : properties.Length;

                int index = 0;

                var model = new T();

                for (int row = 2; row <= rowCount; row++)
                {
                    for (int col = 1; col <= ColCount; col++)
                    {
                        var prop = properties[index];

                        if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                        {
                            try
                            {
                                var date = worksheet.Cells[row, col].GetValue<DateTime?>();

                                if (date.HasValue)
                                {
                                    model.SetPropValue(properties[index].Name, date.Value);
                                }
                            }
                            catch
                            {
                                model.SetPropValue(properties[index].Name, null);
                            }

                        }
                        else if (prop.PropertyType == typeof(int))
                        {
                            try
                            {
                                model.SetPropValue(properties[index].Name, worksheet.Cells[row, col].GetValue<int>());
                            }
                            catch
                            {
                                model.SetPropValue(properties[index].Name, int.MinValue);
                            }
                        }
                        else if (prop.PropertyType == typeof(float))
                        {
                            try
                            {
                                model.SetPropValue(properties[index].Name, worksheet.Cells[row, col].GetValue<float>());
                            }
                            catch
                            {
                                model.SetPropValue(properties[index].Name, float.MinValue);
                            }
                        }
                        else if (prop.PropertyType == typeof(decimal))
                        {
                            try
                            {

                                model.SetPropValue(properties[index].Name, worksheet.Cells[row, col].GetValue<decimal>());
                            }
                            catch
                            {
                                model.SetPropValue(properties[index].Name, decimal.MinValue);
                            }
                        }
                        else if (prop.PropertyType == typeof(string))
                        {
                            try
                            {
                                model.SetPropValue(properties[index].Name, worksheet.Cells[row, col].GetValue<string>().Trim());
                            }
                            catch
                            {
                                model.SetPropValue(properties[index].Name, string.Empty);
                            }
                        }
                        else
                        {
                            var value = Convert.ChangeType(worksheet.Cells[row, col].GetValue<object>(), prop.PropertyType);

                            model.SetPropValue(properties[index].Name, value);
                        }

                        index++;

                        if (index == ColCount)
                        {
                            index = 0;

                            table.Add(model);

                            model = new T();

                        }
                    }
                }

                return table;
            }
            catch (Exception ex)
            {
                throw new ExcelException(ex.Message);
            }
        }
    }
}
