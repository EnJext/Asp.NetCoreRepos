using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using System.IO;

namespace WebApplication1Core
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _Logger;

        public LoggingMiddleware(RequestDelegate next,
            ILoggerFactory LoggerFactory)
        {
            _next = next;
            LoggerFactory.AddFile(Directory.GetCurrentDirectory() + @"/log.txt");
            _Logger = LoggerFactory.CreateLogger<FileLogger>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string Cookies = "Cookies : \n";

            foreach(var cookie in context.Request.Cookies)
            {
                Cookies += " key : " + cookie.Key + " <--> Value : " + cookie.Value + "\n";
            }

            _Logger.LogInformation(Cookies);
            await _next.Invoke(context);
        }
    }

    public static class LogingMiddlewareExtension
    {
        public static IApplicationBuilder UseLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoggingMiddleware>();
        }
    }
}


