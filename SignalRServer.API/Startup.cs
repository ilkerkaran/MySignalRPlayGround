using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SignalRServer.API.Hubs;
using SignalRServer.API.Services;

namespace SignalRServer.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDependencies(services);


            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
                {
                    builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowAnyOrigin()
                        .AllowCredentials();
                }));
            services.AddSignalR();
        }

        private void ConfigureDependencies(IServiceCollection services)
        {
            services.AddSingleton<NewsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("CorsPolicy");

            app.UseSignalR(routes =>
            {
                routes.MapHub<LoopyHub>("/loopy");
                routes.MapHub<NewsHub>("/news");
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
