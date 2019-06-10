using System.Threading;
using System.Threading.Tasks;
using Demo.EventBus.Abstractions;
using Demo.EventBus.HostServices;
using Demo.NotificationService.IntegrationEvents.EventHandling;
using Demo.NotificationService.IntegrationEvents.Events;
using Microsoft.Extensions.Logging;

namespace Demo.NotificationService.Background
{
    public class ConsumeRabbitMQHostedService : BaseConsumeHostService
    {
        public ConsumeRabbitMQHostedService(IEventBus eventBus, ILogger<BaseConsumeHostService> logger) : base(eventBus, logger)
        {
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _eventBus.Subscribe<NotificationIntegrationEvent, NotificationIntegrationEventHandler>();

            return Task.CompletedTask;
        }
    }
}
