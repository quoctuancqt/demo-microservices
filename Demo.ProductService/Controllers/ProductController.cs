using Core.Repositories;
using Demo.Infrastructure.Proxies;
using Demo.ProductService.DTO;
using Demo.ProductService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Demo.ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product, ProductContext> _repository;
        private readonly GatewayApiClient _gatewayApiClient;

        public ProductController(IRepository<Product, ProductContext> repository, GatewayApiClient gatewayApiClient)
        {
            _repository = repository;
            _gatewayApiClient = gatewayApiClient;
            _gatewayApiClient.SetToken();
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
            var entity = dto.CreateEntity();

            _repository.Add(entity);

            await _repository.UnitOfWork.SaveChangesAsync();

            var response = await _gatewayApiClient.PostAsJsonAsync("/notification/notify", entity);

            response.EnsureSuccessStatusCode();

            return new OkObjectResult(entity);
        }
    }
}
