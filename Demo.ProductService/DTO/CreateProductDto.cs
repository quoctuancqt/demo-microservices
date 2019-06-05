using Demo.ProductService.Models;

namespace Demo.ProductService.DTO
{
    public class CreateProductDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string CategoryId { get; set; }

        public Product CreateEntity()
        {
            return new Product
            {
                CategoryId = CategoryId,
                Name = Name,
                Description = Description,
                Price = Price
            };
        }
    }
}
