using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
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

        // Для запросов браузера
        [HttpGet]
        public async Task<ActionResult> GetAsync([FromQuery] string input, [FromQuery] bool ignoreSymbols = false)
        {
            // Искусственно замедляем время обработки запроса
            await Task.Delay(1000);
            return Ok(IsPalindrome(input, ignoreSymbols) ? 1 : 0);
        }

        // Для запросов клиента
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] string input)
        {
            await Task.Delay(1000);
            return Ok(IsPalindrome(input, false) ? 1 : 0);
        }


        private bool IsPalindrome(string str, bool ignoreSymbols = true)
        {
            if (ignoreSymbols)
            {
                // Убираем лишние символы. Так строка вида "a.ba" может считаться палиндромом
                var except = "!?.,~ @#$%^&*<>()[]{}:;`'\"";
                str = new string(str.Where(c => !except.Contains(c)).ToArray());
            }
            for (int i = 0; i < str.Length / 2; i++)
            {
                if (str[i] != str[str.Length - i - 1]) return false;
            }
            return true;
        }
    }
}
