using Core.Entities;
using System.Collections.Generic;

namespace Demo.ProductService.Models
{
    public class Category : EntityBase, IEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
