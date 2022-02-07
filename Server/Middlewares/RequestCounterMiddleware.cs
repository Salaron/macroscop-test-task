using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Middlewares
{
    /// <summary>
    /// Middleware для контроля количества обрабатываемых запросов сервером
    /// </summary>
    public class RequestCounterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly int _maxRequests;
        private int _currentRequests;

        public RequestCounterMiddleware(RequestDelegate next, int maxRequests)
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
                    // Все места заняты, возвращаем Server Busy
                    httpContext.Response.StatusCode = 503;
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

    public static class RequestCounterExtension
    {
        public static IApplicationBuilder UseRequestCounter(this IApplicationBuilder builder, int maxRequests)
        {
            return builder.UseMiddleware<RequestCounterMiddleware>(maxRequests);
        }
    }
}
