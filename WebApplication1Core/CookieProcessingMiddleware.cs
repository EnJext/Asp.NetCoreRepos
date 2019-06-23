using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace WebApplication1Core
{
    public class CookieProcessingMiddleware
    {
        private readonly RequestDelegate _next;

        public CookieProcessingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Cookies.ContainsKey("name"))
            {
                string Name = context.Request.Cookies["name"];
            }
            else
            {
                context.Response.Cookies.Append("name", "Steve");
            }
            await _next.Invoke(context);
        }
    }

    public static class CookieProcessingExtension
    {
        public static IApplicationBuilder UseCookieProcessing(this IApplicationBuilder Builder)
        {
            return Builder.UseMiddleware<CookieProcessingMiddleware>();
        }
    }
}
