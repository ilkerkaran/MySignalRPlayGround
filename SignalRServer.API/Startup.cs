using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using SignalRServer.API.Hubs;
using SignalRServer.API.Security;
using SignalRServer.API.Services;
using SignalRServer.API.ViewModels;

namespace SignalRServer.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
                {
                    builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowAnyOrigin()
                        .AllowCredentials();
                }));


            ConfigureDependencies(services);
            ConfigureJwtAuthService(services);


            services.AddSignalR().AddAzureSignalR();
            services.AddMvc();
        }

        private void ConfigureJwtAuthService(IServiceCollection services)
        {
            var keyByteArray = Encoding.ASCII.GetBytes("this is a custom Secret key for authnetication, Hell YEAH!!!");
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = signingKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                // Validate token expiration
                // ValidateLifetime = true,
ValidIssuer="osman",
ValidAudience ="osman"
                // ClockSkew = TimeSpan.Zero
            };
            var jwtBearerEvents = new JwtBearerEvents()
            {
                OnMessageReceived = context =>
                {
                    if (context.Request.Path.ToString().StartsWith("/hub/", StringComparison.InvariantCultureIgnoreCase) && context.Request.Query.TryGetValue("access_token", out StringValues token))
                    {
                        context.Token = token;
                    }
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    var te = context.Exception;
                    return Task.CompletedTask;
                }
            };
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
                .AddJwtBearer(o =>
                {
                    
                    o.RequireHttpsMetadata = false;
                    o.Events = jwtBearerEvents;
                    o.TokenValidationParameters = tokenValidationParameters;
                });
        }

        private void ConfigureDependencies(IServiceCollection services)
        {
            services.AddSingleton<NewsService>();
            services.AddSingleton<NewsHub>();
            services.AddSingleton<UserService>();
            services.AddScoped<JwtFactory>();
            services.AddScoped<CredentialsViewModel>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("CorsPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();


            app.UseAzureSignalR(routes =>
            {
                routes.MapHub<LoopyHub>("/hub/loopy");
                routes.MapHub<NewsHub>("/hub/news");
            });
            app.UseMvc();
        }
    }
}
