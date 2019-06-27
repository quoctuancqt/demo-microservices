using Demo.EventBus.Abstractions;
using Demo.WorkerService.IntegrationEvents.Events;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Demo.WorkerService.IntegrationEvents.EventHandling
{
    public class NotificationIntegrationEventHandler : IIntegrationEventHandler<NotificationIntegrationEvent>
    {
        public async Task Handle(NotificationIntegrationEvent @event)
        {
            await Task.CompletedTask;

            Console.WriteLine($"Message Received: {JsonConvert.SerializeObject(@event)}");
        }
    }
}
