using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BrunUI
{
    public class BrunUIMiddleware
    {
        private readonly RequestDelegate _next;
        ILogger<BrunUIMiddleware> _logger;
        public BrunUIMiddleware(RequestDelegate next, ILogger<BrunUIMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var query = context.Request.Path.ToString();
            if (string.IsNullOrEmpty(query))// 路径为 /brun
            {
                context.Response.Redirect("/brun/index.html");
                return;
            }
            string[] staticEnds = new string[] { ".js", ".html", ".css", ".svg", ".ico" };
            for (int i = 0; i < staticEnds.Length; i++)
            {
                if (query.EndsWith(staticEnds[i]))
                {
                    string sourceKey = query.Substring(1);//删除开头的/
                    string content = "";
                    if (staticEnds[i] == ".svg" || staticEnds[i] == ".ico")
                    {
                        byte[] buffer = (byte[])Dist.ResourceManager.GetObject(sourceKey);
                        if (buffer == null)
                        {
                            throw new Exception($"path:{query},sourceKey:{sourceKey}");
                        }
                        else
                        {
                            context.Response.Headers.Add("Content-Type", "image/svg+xml");
                            await context.Response.Body.WriteAsync(buffer, 0, buffer.Length);
                            return;
                        }
                    }
                    else
                    {
                        content = Dist.ResourceManager.GetString(sourceKey);
                    }
                    if (content == null)
                    {
                        throw new Exception(String.Format("can not find not this file:{0}", sourceKey));
                        //_logger.LogError("can not find not this file:{0}",sourceKey);
                        //await context.Response.WriteAsync($"path:{query},sourceKey:{sourceKey}");
                    }
                    else
                    {

                        await context.Response.WriteAsync(content);
                    }
                    return;
                }
            }
            await _next.Invoke(context);
        }
    }
}