namespace CExcel.Export
{
    using CExcel.Attributes;
    using System.Collections.Generic;

    public interface IExportHeaderGetter
    {
        Dictionary<string, ExportInfoAttribute> Get<T>();
    }
}
