using Core.Repositories;
using Demo.EventBus.Abstractions;
using Demo.Infrastructure.Proxies;
using Demo.ProductService.DTO;
using Demo.ProductService.IntegrationEvents.Events;
using Demo.ProductService.Models;
using Demo.SFCommunication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Newtonsoft.Json;
using System;
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
        private readonly IHttpCommunicationClientFactory _clientFactory;

        public ProductController(IRepository<Product, ProductContext> repository,
            GatewayApiClient gatewayApiClient
            , IEventBus eventBus
            , IHttpCommunicationClientFactory clientFactory
            )
        {
            _repository = repository;
            _eventBus = eventBus;
            _clientFactory = clientFactory;
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

        [HttpPost("push-to-service")]
        public async Task<IActionResult> PushNotifyToService([FromBody] CreateProductDto dto)
        {
            var result = string.Empty;

            var client = new ServicePartitionClient<HttpCommunicationClient>(
                _clientFactory, new Uri("fabric:/Microservices.DemoApplication/Demo.NotificationService"));

            await client.InvokeWithRetryAsync(async x =>
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString();

                if (!string.IsNullOrEmpty(token))
                {
                    var beareToken = token.Split("Bearer ")[1];

                    x.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", beareToken);

                    var resp = await x.HttpClient.PostAsync("/api/notification/notify", ObjToHttpContent(dto));

                    resp.EnsureSuccessStatusCode();

                    result = await resp.Content.ReadAsStringAsync();
                }
            });

            return Ok(result);
        }

        private StringContent ObjToHttpContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }
    }
}
