using Demo.EventBus.Abstractions;
using Demo.Infrastructure.MongoDb;
using Demo.ProductService.IntegrationEvents.Events;
using System.Threading.Tasks;

namespace Demo.ProductService.IntegrationEvents.EventHandling
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
            await Task.CompletedTask;
            //await _mongoFactory.GetCollection<Notification>()
            //    .InsertOneAsync(new Notification(@event.Name, @event.Description, @event.Price, @event.CategoryId));
        }
    }
}
