using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SurveyPlanLodgement.Web.Data;
using SurveyPlanLodgement.Web.Helpers;
using SurveyPlanLodgement.Web.Models;
using SurveyPlanLodgement.Web.Repository;
using SurveyPlanLodgement.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyPlanLodgement.Web
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
            //Method to add database connection
            services.AddDbContext<SPLDataContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllersWithViews();

            //Method to configure the Identity service
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<SPLDataContext>().AddDefaultTokenProviders();

            //Configure password constraints
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                //Configure Login constraints
                options.SignIn.RequireConfirmedEmail = true;
            });

            //Configure Login page redirection
            services.ConfigureApplicationCookie(config =>
                {
                    config.LoginPath = Configuration["Application:LoginPath"];
                });

#if DEBUG
                services.AddRazorPages().AddRazorRuntimeCompilation();

                //Uncomment to disable client side validations in debug mode
                //    .AddViewOptions(options =>
                //{
                //    options.HtmlHelperOptions.ClientValidationEnabled = false;
                //});
#endif
                services.AddScoped<IAccountRepository, AccountRepository>();
                services.AddScoped<IRolesRepository, RolesRepository>();
                services.AddScoped<ILodgementRepository, LodgementRepository>();
                services.AddScoped<IStatusRepository, StatusRepository>();

                //Configure User Claims
                services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();

                //Configure custom UserService
                services.AddScoped<IUserService, UserService>();

                //Configure custom EmailService
                services.AddScoped<IEmailService, EmailService>();

                //Configure Email Service
                services.Configure<SMTPConfigModel>(Configuration.GetSection("SMTPConfig"));
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

            app.UseRouting();

            //Enable Identity
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
