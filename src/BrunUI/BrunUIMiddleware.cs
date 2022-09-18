using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace BrunUI
{
    /// <summary>
    /// 静态资源
    /// </summary>
    public class BrunUIMiddleware
    {
        private readonly RequestDelegate _next;
        ILogger<BrunUIMiddleware> _logger;
        public static string[] FileNames;
        public static Dictionary<string, byte[]> DistFiles = new Dictionary<string, byte[]>();
        public BrunUIMiddleware(RequestDelegate next, ILogger<BrunUIMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var query = context.Request.Path.ToString().ToLower();
            if (string.IsNullOrEmpty(query))// 路径为 /brun
            {
                context.Response.Redirect("/brun/index.html");
                return;
            }
            //_logger.LogDebug(string.Join("\n",FileNames));
            _logger.LogDebug("query:"+query);//  /index.html
            string sourcekey = "BrunUI.Resources." + query.Substring(1);
            if (DistFiles.ContainsKey(sourcekey))
            {
                if (sourcekey.EndsWith(".svg"))
                {
                    context.Response.Headers.TryAdd("Content-Type", "image/svg+xml");
                }
                else if(sourcekey.EndsWith(".js"))
                {
                    context.Response.Headers.TryAdd("Content-Type", "application/javascript");
                }else if (sourcekey.EndsWith(".css"))
                {
                    context.Response.Headers.TryAdd("Content-Type", "text/css");
                }else if (sourcekey.EndsWith(".ico"))
                {
                    context.Response.Headers.TryAdd("Content-Type", "image/x-icon");
                }
                else
                {
                    context.Response.Headers.TryAdd("Content-Type", "text/html");
                }
                await context.Response.Body.WriteAsync(DistFiles[sourcekey]);
                return;
            }
            await _next.Invoke(context);
        }
        
        // public async Task InvokeAsync(HttpContext context)
        // {
        //     var query = context.Request.Path.ToString();
        //     if (string.IsNullOrEmpty(query))// 路径为 /brun
        //     {
        //         context.Response.Redirect("/brun/index.html");
        //         return;
        //     }
        //     string[] staticEnds = new string[] { ".js", ".html", ".css",".png", ".svg", ".ico" };
        //     for (int i = 0; i < staticEnds.Length; i++)
        //     {
        //         if (query.EndsWith(staticEnds[i]))
        //         {
        //             string sourceKey = query.Substring(1);//删除开头的/
        //             string content = "";
        //             if (staticEnds[i] == ".svg" || staticEnds[i] == ".ico")
        //             {
        //                 byte[] buffer = (byte[])Dist.ResourceManager.GetObject(sourceKey);
        //                 if (buffer == null)
        //                 {
        //                     throw new Exception($"path:{query},sourceKey:{sourceKey}");
        //                 }
        //                 else
        //                 {
        //                     context.Response.Headers.Add("Content-Type", "image/svg+xml");
        //                     await context.Response.Body.WriteAsync(buffer, 0, buffer.Length);
        //                     return;
        //                 }
        //             }
        //             else
        //             {
        //                 content = Dist.ResourceManager.GetString(sourceKey);
        //             }
        //             if (content == null)
        //             {
        //                 throw new Exception(String.Format("can not find not this file:{0}", sourceKey));
        //                 //_logger.LogError("can not find not this file:{0}",sourceKey);
        //                 //await context.Response.WriteAsync($"path:{query},sourceKey:{sourceKey}");
        //             }
        //             else
        //             {
        //
        //                 await context.Response.WriteAsync(content);
        //             }
        //             return;
        //         }
        //     }
        //     await _next.Invoke(context);
        // }
    }
}
// BrunUI.dist.icons.icon-128x128.png
// BrunUI.dist.icons.icon-192x192.png
// BrunUI.dist.icons.icon-512x512.png
// BrunUI.dist.asset-manifest.json
// BrunUI.dist.CNAME
// BrunUI.dist.favicon.ico
// BrunUI.dist.index.html
// BrunUI.dist.logo.svg
// BrunUI.dist.pro_icon.svg
// BrunUI.dist.umi.css
// BrunUI.dist.umi.js
