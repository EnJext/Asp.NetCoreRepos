using System;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Primitives;


namespace WebApplication1Core
{
    public class RequestCultureMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string pas;

        public RequestCultureMiddleware(RequestDelegate next, string pas)
        {
            _next = next;
            this.pas = pas;
        }

        public async Task InvokeAsync(HttpContext context, IApplicationBuilder app)
        {
            StringValues token; 

            if (context.Request.Query.ContainsKey("token"))
            {
                token = context.Request.Query["token"];
            }

            if (token == pas)
            {
                await _next(context);
            }
            else
            {
                await context.Response.WriteAsync("invalid token");
            }
        }
    }

    public static class RequestCultureMiddlewareExtension
    {
        public static IApplicationBuilder UseRequestCulture(
            this IApplicationBuilder builder, string pas)
        { 
            return builder.UseMiddleware<RequestCultureMiddleware>(pas);
        }
    }

}
