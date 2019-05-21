using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ProductsAPIServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Surface Book 2", "Mac Book Pro" };
        }
    }
}
