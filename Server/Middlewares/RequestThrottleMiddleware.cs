using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Middlewares
{
    /// <summary>
    /// Middleware для контроля количества обрабатываемых запросов сервером
    /// </summary>
    public class RequestThrottleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly int _maxRequests;
        private int _currentRequests;

        public RequestThrottleMiddleware(RequestDelegate next, int maxRequests)
        {
            _next = next;
            _maxRequests = maxRequests;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // Проверяем можем ли мы обслужить текущий запрос
                if (Interlocked.Increment(ref _currentRequests) > _maxRequests)
                {
                    // Все места заняты, возвращаем Too Many Requests
                    httpContext.Response.StatusCode = 429;
                } else
                {
                    // Передаём запрос дальше
                    await _next(httpContext);
                }
            }
            finally
            {
                // Уменьшаем счётчик запросов, после выполнения
                Interlocked.Decrement(ref _currentRequests);
            }
        }
    }

    public static class RequestThrottleExtension
    {
        public static IApplicationBuilder UseRequestThrottle(this IApplicationBuilder builder, int maxRequests)
        {
            return builder.UseMiddleware<RequestThrottleMiddleware>(maxRequests);
        }
    }
}
