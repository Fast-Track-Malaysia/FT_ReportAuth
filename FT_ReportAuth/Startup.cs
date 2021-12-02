using FT_ReportAuth.Areas.Identity;
using FT_ReportAuth.Data;
using FT_ReportAuth.Services;
using FT_SpReport.CoreBusiness.Helpers;
using FT_SpReport.CoreBusiness.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FT_ReportAuth
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequiredLength = StaticValues.PasswordLength;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // requires
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration.GetSection(
                                        AuthMessageSenderOptions.AuthMessageSender));

            services.AddAuthorization(options =>
            {
                options.AddPolicy(StaticValues.RequireAdminPolicy, policy =>
                    policy.RequireRole(StaticValues.AdministratorRole));
                options.AddPolicy(StaticValues.RequireManagerPolicy, policy =>
                    policy.RequireRole(StaticValues.AdministratorRole, StaticValues.ManagerRole));
                options.AddPolicy(StaticValues.RequireUserPolicy, policy =>
                    policy.RequireRole(StaticValues.AdministratorRole, StaticValues.ManagerRole, StaticValues.UserRole));
            });

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            services.AddSingleton<WeatherForecastService>();

            services
              .AddScoped<IHttpService, HttpService>()
              .AddScoped<ILocalStorageService, LocalStorageService>()
              .AddScoped<SpModelService>()
              .AddScoped<SpReportService>()
              .AddScoped<LookupModelService>()
              .AddScoped<ReportModelService>();

            services.AddScoped<BrowserService>(); // scoped service

            services.AddScoped(x => {
                var apiUrl = new Uri(Configuration.GetSection("ApiUrl").Value);

                return new System.Net.Http.HttpClient() { BaseAddress = apiUrl };
            });

            services.AddSingleton(Configuration);
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
