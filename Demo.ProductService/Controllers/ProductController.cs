using Core.Repositories;
using Demo.ProductService.DTO;
using Demo.ProductService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product, ProductContext> _repository;
        private readonly IRepository<Category, ProductContext> _categoryRepository;

        public ProductController(IRepository<Product, ProductContext> repository,
            IRepository<Category, ProductContext> categoryRepository)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var products = _repository.FindAll();

            return new OkObjectResult(products);
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var product = _repository.FindBy(id);
            return new OkObjectResult(product);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProductDto dto)
        {
            _repository.Add(dto.CreateEntity());

            await _repository.UnitOfWork.SaveChangesAsync();

            return Ok("Success");
        }
    }
}
