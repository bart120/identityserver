using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.RequireHttpsMetadata = false;
                    options.Audience = "apitest";
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(options =>
            {
                /*options.AddPolicy("scope policy", p =>
                {
                    p.RequireClaim("scope", "api_meteo_scope");
                });*/
                options.AddPolicy("MVCCLIENT", policy => policy.RequireClaim("client_id", new string[] { "mobileapp" }));
            });

            services.AddAllowedHosts();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAllowedHosts(this IServiceCollection services)
        {
            List<string> hosts = new List<string> { "http://localhost:4200", "https://mondomane.com", "http://mon.autre.domaine.com" };



            services.AddCors(o =>
            {
                o.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins(hosts.ToArray());
                    //builder.AllowAnyOrigin(); // *
                    builder.AllowAnyMethod(); // toutes les methodes HHTP (GET, POST, PUT, DELETE, OPTION ...)
                    //builder.AllowAnyHeader();
                });
            });

            return services;
        }
    }
}
