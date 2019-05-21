namespace Resilience.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Resilience.Factory;

    public static class ResilienceHttpExtension
    {
        public static IServiceCollection RegisterResilienceHttp(this IServiceCollection services, bool useResilientHttp = false)
        {
            if (useResilientHttp)
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
    }
}
