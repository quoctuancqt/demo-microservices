namespace CFileManager
{
    using CFileManager.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class CFileManagerExtension
    {
        public static IServiceCollection RegisterCFileManager(this IServiceCollection services)
        {
            services.AddSingleton<IFileManager, FileManager>();

            return services;
        }
    }
}
