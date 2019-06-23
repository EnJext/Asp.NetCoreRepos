using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace WebApplication1Core
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(3600);
            });
            int x = 9;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.Map("/error", appBuilder =>
                 {
                     app.Run(async (context) => await context.Response.WriteAsync("Error"));
                 });
            }

            app.UseStatusCodePages();
            app.UseLogging();
            app.UseSession();
            app.UseSessionHendler();
            app.UseCookieProcessing();

            app.Map("/fileserver", FileServerRequestHandler);

            var DefFileOptions = new DefaultFilesOptions();
            DefFileOptions.DefaultFileNames.Clear();
            DefFileOptions.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(DefFileOptions);

            app.UseStaticFiles();
        }

        private void FileServerRequestHandler(IApplicationBuilder app)
        {
            app.UseRequestCulture("1111");

            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory()
                + @"/wwwroot"),
                RequestPath = @"/wwwroot",
                EnableDirectoryBrowsing = true,
                EnableDefaultFiles = false
            });
        }
    }
}
