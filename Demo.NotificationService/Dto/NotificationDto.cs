using MessagePack;

namespace Demo.NotificationService.Dto
{
    [MessagePackObject]
    public class NotificationDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryId { get; set; }
    }
}
