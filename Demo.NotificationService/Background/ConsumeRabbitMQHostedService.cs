using System.Threading;
using System.Threading.Tasks;
using Demo.EventBus.Abstractions;
using Demo.NotificationService.IntegrationEvents.EventHandling;
using Demo.NotificationService.IntegrationEvents.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Demo.NotificationService.Background
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
            _eventBus.Subscribe<NotificationIntegrationEvent, NotificationIntegrationEventHandler>();

            return Task.CompletedTask;
        }
    }
}
