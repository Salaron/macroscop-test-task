using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PingController : ControllerBase
    {
        private readonly ILogger<IsPalindromeController> _logger;

        public PingController(ILogger<IsPalindromeController> logger)
        {
            _logger = logger;
        }

        // Метод для проверки сервера клиентом
        [HttpGet]
        public ActionResult Get()
        {
            return Ok("pong");
        }
    }
}
