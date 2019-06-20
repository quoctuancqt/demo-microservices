using MongoDB.Bson;

namespace Demo.WorkerService.Models
{
    public class Notification
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryId { get; set; }

        public Notification(string name, string description, decimal price, string categoryId)
        {
            Name = name;
            Description = description;
            Price = price;
            CategoryId = categoryId;
        }
    }
}
