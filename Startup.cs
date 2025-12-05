using MasterApplication.DAL;
using MasterApplication.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication
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
            services.AddControllers().AddNewtonsoftJson(x =>
                                x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<FormsAuthentication, FormsAuthentication>();
            services.AddSingleton<CommonClass, CommonClass>();
            services.Configure<MyAppSettings>(Configuration.GetSection("AppSettings"));
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NCaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXhecnRWQ2VYUUd+WkpWYU8=");
            services.AddSingleton<DBAccess, DBAccess>();
            services.AddSingleton<DependancyInjection, DependancyInjection>();
            services.AddSession(options =>
            {
                //options.IdleTimeout = TimeSpan.FromMinutes(15);
                options.IdleTimeout = TimeSpan.FromMinutes(15);
            });

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(1);
            });

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.MinimumSameSitePolicy = SameSiteMode.Strict;
            //    options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
            //    options.Secure = CookieSecurePolicy.Always;
            //});

            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();

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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

            }
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseCookiePolicy();

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                context.Response.Headers.Add("Referrer-Policy", "no-referrer");
                context.Response.Headers.Add("Permissions-Policy", "geolocation=(), microphone=(), camera=()");
                context.Response.Headers.Add("Strict-Transport-Security", "max-age=120; includeSubDomains");
                //context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; style-src 'self' 'unsafe-inline';");
                context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
                context.Response.Headers.Add("Content-Security-Policy", "default-src *; script-src * 'unsafe-inline'; style-src * 'unsafe-inline';");
                await next();
            });
            app.UseMiddleware<PortalMiddleware>();
            app.UseAuthorization();

            app.UseMvc(routes =>
            {

                routes.MapRoute(
              name: "areas",
              template: "{area:exists}/{controller}/{action}/{id?}"
            );
                routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");

            });



            //            app.UseEndpoints(endpoints =>
            //            {

            //               endpoints.MapControllerRoute(
            //               name: "MyArea",
            //               pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}"); //This is Administrator route. You can you {id} and other parameters which you want
            ////               endpoints.MapRazorPages();

            //               //endpoints.MapControllerRoute(
            //               //     name: "default",
            //               //     pattern: "{controller=Home}/{action=Index}"); // This route is for Controllers which are situated in project controller folder
            //            });

        }
    }
}
