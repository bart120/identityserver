using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IdentityServer.Data;
using IdentityServer.ProfilServices;
using IdentityServer.Validators;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer
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
            var migrationAssemblyName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddControllersWithViews();

            services.AddDbContext<AuthenticationDbContext>(o =>
            {
                o.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"));
            });

            var builder = services.AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = "/login";
                options.UserInteraction.LogoutUrl = "/logout";
            })
                .AddConfigurationStore(o =>
                {
                    //o.DefaultSchema = "configuration";
                    o.ConfigureDbContext = b => b.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"),
                        sql => sql.MigrationsAssembly(migrationAssemblyName));
                })
                .AddOperationalStore(o =>
                {
                    //o.DefaultSchema = "grant";
                    o.ConfigureDbContext = b => b.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"),
                        sql => sql.MigrationsAssembly(migrationAssemblyName));
                });
            builder.AddResourceOwnerValidator<ROClientValidator>();
            builder.AddDeveloperSigningCredential();

            services.AddTransient<IProfileService, ProfileService>();

            services.AddAuthentication();

            services.AddAllowedHosts();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseIdentityServer();

            app.UseCors();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
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
