namespace CoreDistributedCache
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DistributedCacheExtensions
    {
        public static IServiceCollection AddSqlServerCache(this IServiceCollection services,
            IConfiguration configuration)
        {
            var sqlDistCache = configuration.GetSection("SqlDistCache");

            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = sqlDistCache.GetValue<string>("ConnectionString");
                options.SchemaName = "dbo";
                options.TableName = sqlDistCache.GetValue<string>("TableName");
            });

            services.AddSingleton<IDistributedCacheService, DistributedCacheService>();

            return services;
        }
    }
}
