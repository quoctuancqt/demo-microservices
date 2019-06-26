using AutoMapper;
using Core.Interfaces;
using Core.Logging;
using Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Extensions
{
    public static class RegisterServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

            services.AddScoped(typeof(IReadOnlyRepository<,>), typeof(ReadOnlyRepository<,>));

            services.Scan(scan =>
            {
                scan.FromCallingAssembly()
                        .AddClasses()
                        .AsMatchingInterface();
            });

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
