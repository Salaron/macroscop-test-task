using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IsPalindromeController : ControllerBase
    {
        private readonly ILogger<IsPalindromeController> _logger;

        public IsPalindromeController(ILogger<IsPalindromeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync([FromQuery] string input)
        {
            await Task.Delay(1000);
            // TODO: проверка на палиндром
            return Ok(input);
        }
    }
}
