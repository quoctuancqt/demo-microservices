namespace CExcel
{
    using CExcel.Export;
    using CExcel.Import;
    using CExcel.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class CExcelExtension
    {
        public static IServiceCollection RegisterCExcel(this IServiceCollection services)
        {
            services.AddSingleton<IImportHeaderGetter, ImportHeaderGetter>();
            services.AddSingleton<IExportHeaderGetter, ExportHeaderGetter>();
            services.AddSingleton<IExcelCommonExportService, ExcelCommonExportService>();
            services.AddSingleton<IExcelService, ExcelService>();

            return services;
        }
    }
}
