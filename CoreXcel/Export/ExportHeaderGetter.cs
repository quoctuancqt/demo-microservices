namespace CExcel.Export
{
    using CExcel.Attributes;
    using System.Collections.Generic;
    using System.Linq;

    public class ExportHeaderGetter : IExportHeaderGetter
    {
        public Dictionary<string, ExportInfoAttribute> Get<T>()
        {
            var exportInfoMap = new Dictionary<string, ExportInfoAttribute>();
            var type = typeof(T);

            foreach (var prop in type.GetProperties())
            {
                var exportInfoAttribute = (ExportInfoAttribute)prop
                    .GetCustomAttributes(typeof(ExportInfoAttribute), false)
                    .FirstOrDefault();

                if (exportInfoAttribute != null)
                {
                    exportInfoMap.Add(prop.Name, exportInfoAttribute);
                }
            }

            exportInfoMap.OrderBy(kpv => kpv.Value.Order);

            return exportInfoMap;
        }
    }
}
