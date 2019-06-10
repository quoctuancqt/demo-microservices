using System.Threading;
using System.Threading.Tasks;
using Demo.EventBus.Abstractions;
using Demo.EventBus.HostServices;
using Demo.ProductService.IntegrationEvents.EventHandling;
using Demo.ProductService.IntegrationEvents.Events;
using Microsoft.Extensions.Logging;

namespace Demo.ProductService.Background
{
    public class ConsumeRabbitMQHostedService : BaseConsumeHostService
    {
        public ConsumeRabbitMQHostedService(IEventBus eventBus, ILogger<BaseConsumeHostService> logger) : base(eventBus, logger)
        {
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            _eventBus.Subscribe<NotificationIntegrationEvent, NotificationIntegrationEventHandler>();

            return Task.CompletedTask;
        }
    }
}
