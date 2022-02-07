using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

using Server.dtos;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IsPalindromeController : ControllerBase
    {
        // Для запросов браузера
        [HttpGet]
        public async Task<ActionResult> GetAsync([FromQuery] string input, [FromQuery] bool ignoreSymbols = false)
        {
            // Искусственно замедляем время обработки запроса
            await Task.Delay(1000);
            PalindromeResultDTO response = new();
            response.Result = IsPalindrome(input, ignoreSymbols);
            return Ok(response);
        }

        // Для запросов клиента
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] PalindromeCheckDTO request)
        {
            await Task.Delay(1000);
            PalindromeResultDTO response = new();
            response.Result = IsPalindrome(request.Input, request.IgnoreSymbols.GetValueOrDefault());
            return Ok(response);
        }

        private bool IsPalindrome(string str, bool ignoreSymbols = false)
        {
            if (ignoreSymbols)
            {
                // Убираем лишние символы. Так строка вида "a.ba" может считаться палиндромом
                var except = "!?.,~ @#$%^&*<>()[]{}:;`’'\"\n\r\t";
                str = new string(str.Where(c => !except.Contains(c)).ToArray());
            }
            // Делаем всё в lowercase, чтобы можно было проверять целые предложения
            str = str.ToLower();
            for (int i = 0; i < str.Length / 2; i++)
            {
                if (str[i] != str[str.Length - i - 1]) return false;
            }
            return true;
        }
    }
}
