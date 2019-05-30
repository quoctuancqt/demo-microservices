namespace Core.Extensions
{
    using AutoMapper;
    using Core.Interfaces;
    using Core.Logging;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Resilience;
    using Resilience.Factory;
    using System;
    using System.Reflection;

    public static class RegisterServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(UnitOfWorkFactory);

            services.AddSingleton(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            if (configuration.GetValue<string>("UseResilientHttp") == bool.TrueString)
            {
                services.AddTransient<IResilientHttpClientFactory, ResilientHttpClientFactory>();
                services.AddTransient<IHttpClient, ResilientHttpClient>(sp => sp.GetService<IResilientHttpClientFactory>().CreateResilientHttpClient());
            }
            else
            {
                services.AddTransient<IHttpClient, StandardHttpClient>();
            }

            return services;
        }

        private static UnitOfWork.IUnitOfWork UnitOfWorkFactory(IServiceProvider serviceProvider)
        {
            return new UnitOfWork.UnitOfWork<DbContext>(serviceProvider.GetService<IHttpContextAccessor>(), serviceProvider.GetService<DbContext>());
        }
    }
}
