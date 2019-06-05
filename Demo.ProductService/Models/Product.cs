using Core.Entities;

namespace Demo.ProductService.Models
{
    public class Product : EntityBase, IEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
