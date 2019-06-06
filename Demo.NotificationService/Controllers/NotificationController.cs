using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        [HttpPost("notify")]
        public IActionResult Notify([FromBody] IDictionary<string, string> dic)
        {
            Console.WriteLine($"Id: {dic["Id"]}, Name: {dic["Name"]}");

            return Ok();
        }
    }
}
