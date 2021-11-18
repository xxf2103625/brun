using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BrunUI
{
    public class BrunUIMiddleware
    {
        private readonly RequestDelegate _next;
        ILogger<BrunUIMiddleware> _logger;
        public BrunUIMiddleware(RequestDelegate next,ILogger<BrunUIMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var query = context.Request.Query;
            string? type = query["type"][0];
            string? name = query["name"][0];
            //Console.WriteLine($"type:{type},name:{name}");
            _logger.LogWarning($"type:{type},name:{name}");
            await context.Response.WriteAsync($"Query: type:{type},name:{name}");
        }
    }
}