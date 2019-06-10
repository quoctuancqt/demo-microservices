using System.Threading;
using System.Threading.Tasks;
using Demo.EventBus.Abstractions;
using Demo.ProductService.IntegrationEvents.EventHandling;
using Demo.ProductService.IntegrationEvents.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Demo.ProductService.Background
{
    public class ConsumeRabbitMQHostedService : BackgroundService
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<ConsumeRabbitMQHostedService> _logger;
        public ConsumeRabbitMQHostedService(IEventBus eventBus, ILogger<ConsumeRabbitMQHostedService> logger)
        {
            _eventBus = eventBus;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            _eventBus.Subscribe<NotificationIntegrationEvent, NotificationIntegrationEventHandler>();

            return Task.CompletedTask;
        }
    }
}
