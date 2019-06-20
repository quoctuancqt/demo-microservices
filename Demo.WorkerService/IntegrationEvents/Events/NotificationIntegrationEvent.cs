using Demo.EventBus.Events;

namespace Demo.WorkerService.IntegrationEvents.Events
{
    public class NotificationIntegrationEvent : IntegrationEvent
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public string CategoryId { get; set; }

        public NotificationIntegrationEvent(string name, string desctiption, decimal price, string categoryId)
        {
            Name = name;
            Description = desctiption;
            Price = price;
            CategoryId = categoryId;
        }
    }
}
