using Demo.EventBus.Abstractions;
using Demo.EventBus.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Demo.EventBus.Extensions
{
    public static class RegisterEventBus
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            var subscriptionClientName = configuration["EventBus:QueueName"];

            if (configuration["EventBus:AzureServiceBus"] == bool.TrueString)
            {
                services.AddSingleton<IServiceBusPersisterConnection>(sp =>
                {
                    var logger = sp.GetRequiredService<ILogger<DefaultServiceBusPersisterConnection>>();

                    var serviceBusConnectionString = configuration["EventBus:Connection"];
                    var serviceBusConnection = new ServiceBusConnectionStringBuilder(serviceBusConnectionString);

                    return new DefaultServiceBusPersisterConnection(serviceBusConnection, logger);
                });

                services.AddSingleton<IEventBus, EventBusServiceBus>(sp =>
                {
                    var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
                    var logger = sp.GetRequiredService<ILogger<EventBusServiceBus>>();
                    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                    return new EventBusServiceBus(serviceBusPersisterConnection, logger,
                        eventBusSubcriptionsManager, subscriptionClientName, sp);
                });
            }
            else
            {
                services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
                {
                    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();


                    var factory = new ConnectionFactory();

                    if (!string.IsNullOrEmpty(configuration["EventBus:Connection"]))
                    {
                        factory.Uri = new Uri(configuration["EventBus:Connection"]);
                    }
                    else
                    {
                        factory.HostName = configuration["EventBus:HostName"];
                        factory.Port = int.Parse(configuration["EventBus:Port"]);

                        if (!string.IsNullOrEmpty(configuration["EventBus:Username"]))
                        {
                            factory.UserName = configuration["EventBus:Username"];
                        }

                        if (!string.IsNullOrEmpty(configuration["EventBus:Password"]))
                        {
                            factory.Password = configuration["EventBus:Password"];
                        }
                    }

                    var retryCount = 5;
                    if (!string.IsNullOrEmpty(configuration["EventBus:RetryCount"]))
                    {
                        retryCount = int.Parse(configuration["EventBus:RetryCount"]);
                    }

                    return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
                });

                services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
                {
                    var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                    var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                    var retryCount = 5;
                    if (!string.IsNullOrEmpty(configuration["EventBus:RetryCount"]))
                    {
                        retryCount = int.Parse(configuration["EventBus:RetryCount"]);
                    }

                    return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, sp, eventBusSubcriptionsManager, configuration, subscriptionClientName, retryCount);
                });
            }

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            return services;
        }

        public static void RegisterEventBusHandler(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetService<IEventBus>();

            var types = Assembly.GetCallingAssembly().GetExportedTypes();

            var integrationEvents = types.Where(t => t.IsSubclassOf(typeof(IntegrationEvent)) && !t.IsAbstract);

            foreach (var integrationEvent in integrationEvents)
            {
                var handler = types.SingleOrDefault(h => h.Name.Equals($"{integrationEvent.Name}Handler") && (typeof(IIntegrationEventHandler).IsAssignableFrom(h)));

                var sub = typeof(IEventBus).GetMethod("Subscribe");
                var generic = sub.MakeGenericMethod(integrationEvent, handler);
                generic.Invoke(eventBus, null);
            }

            Console.WriteLine("Test");
        }
    }
}
