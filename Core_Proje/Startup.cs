using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Core_Proje
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
            // Add database context
            services.AddDbContext<Context>();

            // Add Identity with custom user and role types
            services.AddIdentity<WriterUser, WriterRole>().AddEntityFrameworkStores<Context>();
            services.AddControllersWithViews();

            // Configure global authorization filter (requires authentication by default)
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                               .RequireAuthenticatedUser()
                               .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            // Register CV summary service with OpenAI API key from configuration
            services.AddScoped<ICvSummaryService>(provider =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                var apiKey = config["OpenAI:ApiKey"];
                return new CvSummaryManager(apiKey);
            });

            services.AddMvc();

            // Configure cookie authentication
            services.AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme);
            // Note: This is commented out - alternative configuration below
            //.AddCookie(x =>
            //{
            //    x.LoginPath = "/AdminLogin/Index/";
            //});

            // Configure application cookie settings (first configuration)
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true; // Prevent JavaScript access
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Cookie expires after 30 minutes
                options.SlidingExpiration = true; // Reset expiration on each request
                options.AccessDeniedPath = "/ErrorPage/Index/";
                options.LoginPath = "/Writer/Login/Index/";
            });

            services.AddMvc();

            // Alternative cookie authentication configuration
            services.AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(x =>
                {
                    x.LoginPath = "/AdminLogin/Index/";
                });

            // Configure application cookie settings (second configuration - might override first)
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(100); // Different timeout (100 minutes)
                options.AccessDeniedPath = "/ErrorPage/Index/";
                options.LoginPath = "/Writer/Login/Index/";
            });

            // Add session services for state management
            services.AddSession();
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
                app.UseHsts(); // HTTP Strict Transport Security
            }

            // Custom error page for 404 errors
            app.UseStatusCodePagesWithReExecute("/ErrorPage/Error404/");
            app.UseHttpsRedirection();
            app.UseStaticFiles(); // Serve static files like CSS, JS, images

            app.UseRouting();

            // Authentication and authorization middleware
            app.UseAuthentication();
            app.UseAuthorization();

            // Enable session middleware
            app.UseSession();

            // Configure endpoint routing
            app.UseEndpoints(endpoints =>
            {
                // Default route for main application
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Default}/{action=Index}/{id?}");

                // Area route for modular sections (like Admin, Writer areas)
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Default}/{action=Index}/{id?}");
            });
        }
    }
}