using Demo.Infrastructure.MongoDb;
using Demo.NotificationService.Dto;
using Demo.NotificationService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly MongoFactory _mongoFactory;

        public NotificationController(MongoFactory mongoFactory)
        {
            _mongoFactory = mongoFactory;
        }

        [HttpPost("notify")]
        public async Task<IActionResult> Notify([FromBody] NotificationDto dto)
        {
            await _mongoFactory.GetCollection<Notification>()
                .InsertOneAsync(new Notification(dto.Name, dto.Description, dto.Price, dto.CategoryId));

            return Ok();
        }
    }
}
