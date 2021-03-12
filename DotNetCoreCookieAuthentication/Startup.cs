using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DotNetCoreCookieAuthentication
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("MyCookieAuth")
                .AddCookie("MyCookieAuth", config => {
                    config.Cookie.Name = "MyCookieAuth";
                    config.LoginPath = "/Home/Authenticate";  //redirect to login page if not authenticated
                });

            services.AddAuthorization(config =>
            {
                config.AddPolicy("PolicyA", new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(ClaimTypes.Email)
                    .RequireClaim("Something")
                    .Build());
            });

            services.AddControllersWithViews();

            services.AddRazorPages(config =>
            {
                config.Conventions.AuthorizePage("/Razor/AuthorizedRazorPage");
                //config.Conventions.AuthorizePage("/Razor/AuthorizedRazorPage", "PolicyNameHere");
                //config.Conventions.AuthorizeFolder("/FolderHere");
                //config.Conventions.AllowAnonymousToPage("/PageHere");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}
