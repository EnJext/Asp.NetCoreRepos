using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Builder;

namespace WebApplication1Core
{
    public class SesionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public SesionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            byte[] Value;
            User user;
            if (context.Session.TryGetValue("user", out Value))
            {
                string JsonValue = context.Session.GetString("user");
                user = JsonConvert.DeserializeObject<User>(JsonValue);
            }
            else
            {
                string JsonValue = JsonConvert.SerializeObject(new User() { Login = "Valik", Pas = "1111" });
                context.Session.SetString("user", JsonValue);
            }
            await _next.Invoke(context);
        }
    }

    public static class SeesionExtension
    {
        public static IApplicationBuilder UseSessionHendler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SesionHandlerMiddleware>();
        }
    }

    public struct User
    {
        public string Login;
        public string Pas;
    }
}
