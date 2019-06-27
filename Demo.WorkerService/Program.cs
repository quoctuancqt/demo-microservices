using Common.Helpers;
using Demo.EventBus.Abstractions;
using Demo.EventBus.Extensions;
using Demo.WorkerService.IntegrationEvents.EventHandling;
using Demo.WorkerService.IntegrationEvents.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo.WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseEnvironment(EnvironmentHelper.Environment)
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

                    services.AddEventBus(configuration);

                    services.AddTransient<NotificationIntegrationEventHandler>();

                    var eventBus = services.BuildServiceProvider().GetService<IEventBus>();
                    eventBus.Subscribe<NotificationIntegrationEvent, NotificationIntegrationEventHandler>();

                    services.AddHostedService<Worker>();
                });
    }
}
