namespace CExcel.Import
{
    using CExcel.Attributes;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class ImportHeaderGetter : IImportHeaderGetter
    {
        public PropertyInfo[] Get<T>()
        {
            var importsMap = new Dictionary<PropertyInfo, ImportAttribute>();
            var type = typeof(T);

            foreach (var prop in type.GetProperties())
            {
                var importAttribute = (ImportAttribute)prop
                    .GetCustomAttributes(typeof(ImportAttribute), false)
                    .FirstOrDefault();

                if (importAttribute != null)
                {
                    importsMap.Add(prop, importAttribute);
                }
            }

            return importsMap
                .OrderBy(kpv => kpv.Value.Order)
                .Select(kvp => kvp.Key).ToArray();
        }
    }
}
