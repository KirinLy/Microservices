using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : Controller
    {
        [HttpPost]
        [ProducesResponseType((int)StatusCodes.Status200OK)]
        public async Task<ActionResult> CreateOrder()
        {
            return Ok();
        }
    }
}
