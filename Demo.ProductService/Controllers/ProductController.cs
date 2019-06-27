using Core.Repositories;
using Demo.EventBus.Abstractions;
using Demo.Infrastructure.Proxies;
using Demo.ProductService.DTO;
using Demo.ProductService.IntegrationEvents.Events;
using Demo.ProductService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
        private readonly IEventBus _eventBus;

        public ProductController(IRepository<Product, ProductContext> repository,
            GatewayApiClient gatewayApiClient
            , IEventBus eventBus
            )
        {
            _repository = repository;
            _eventBus = eventBus;
            _gatewayApiClient = gatewayApiClient;
            _gatewayApiClient.SetToken();
        }

        [HttpGet]
        public IActionResult Get()
        {
            var products = _repository.FindAll().ToList();

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

            using (var client = new HttpClient())
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString();

                if (!string.IsNullOrEmpty(token))
                {
                    var beareToken = token.Split("Bearer ")[1];

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", beareToken);

                    var resp = await client.PostAsync("http://192.168.2.99:5002/api/notification/notify", ObjToHttpContent(entity));

                    resp.EnsureSuccessStatusCode();
                }
            }

            return new OkObjectResult(entity);
        }

        [HttpPost("push-notify")]
        public IActionResult PushNotify([FromBody] CreateProductDto dto)
        {
            _eventBus.Publish(new NotificationIntegrationEvent(dto.Name, dto.Description, dto.Price, dto.CategoryId));

            return Ok("Publish");
        }

        private StringContent ObjToHttpContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }
    }
}
