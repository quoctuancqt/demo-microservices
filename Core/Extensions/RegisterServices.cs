using AutoMapper;
using Core.Interfaces;
using Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Resilience.Extensions;
using System;
using System.Reflection;

namespace Core.Extensions
{
    public static class RegisterServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(UnitOfWorkFactory);

            services.AddSingleton(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.RegisterResilienceHttp(bool.Parse(configuration.GetValue<string>("UseResilientHttp")));

            return services;
        }

        private static UnitOfWork.IUnitOfWork UnitOfWorkFactory(IServiceProvider serviceProvider)
        {
            return new UnitOfWork.UnitOfWork<DbContext>(serviceProvider.GetService<IHttpContextAccessor>(), serviceProvider.GetService<DbContext>());
        }
    }
}
