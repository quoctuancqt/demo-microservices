using Demo.EventBus.Abstractions;
using Demo.Infrastructure.MongoDb;
using Demo.NotificationService.IntegrationEvents.Events;
using Demo.NotificationService.Models;
using System.Threading.Tasks;

namespace Demo.NotificationService.IntegrationEvents.EventHandling
{
    public class NotificationIntegrationEventHandler : IIntegrationEventHandler<NotificationIntegrationEvent>
    {
        private readonly MongoFactory _mongoFactory;

        public NotificationIntegrationEventHandler(MongoFactory mongoFactory)
        {
            _mongoFactory = mongoFactory;
        }

        public async Task Handle(NotificationIntegrationEvent @event)
        {
            await _mongoFactory.GetCollection<Notification>()
                .InsertOneAsync(new Notification(@event.Name, @event.Description, @event.Price, @event.CategoryId));
        }
    }
}
