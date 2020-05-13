using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.DAL;
using WebStore.DomainNew.Entities;
using WebStore.Infrastructure;
using WebStore.Infrastructure.Implementation;
using WebStore.Infrastructure.Interfaces;

namespace WebStore
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //��������� �������, ����������� ��� mvc
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(SimpleActionFilter)); //����������� �� ����
                // �������������� ������� �����������
                //options.Filters.Add(new SimpleActionFilter()); // ����������� �� �������
            });

            services.AddDbContext<WebStoreContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

            // ��������� ���������� �����������.�.�. ������ ��� ����� �� ��������� IEmployeesService ���-� � ����, �� ����������
            // ���-� InMemoryEmployeesService. ������ �������� ��� SingleTon �.�. �������� ���� ��������� InMemoryEmployeesService
            // �� �� ���������� � ��������������� ���� ��������� �� ����� � ������� ����� ����������.
            // Transient ��������, ��� ��������� InMemoryEmployeesService ����� ��������������� ������ ��� ����� � ���� ��� ���������
            // Scoped ��������, ��� ����� ����������� ���� ��������� �� ���� http ������
            services.AddSingleton<IEmployeesService, InMemoryEmployeesService>();
            services.AddScoped<IProductService, SqlProductService>();
            //services.AddTransient<IEmployeesService, InMemoryEmployeesService>();
            //services.AddScoped<IEmployeesService, InMemoryEmployeesService>();

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<WebStoreContext
                >() //�����������! ��������� identity � entityframework core ��� �������� �������� � ������ � �� � �������
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
                {
                    // password settings
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 5;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;

                    // ��� ����� ������ ������������
                    options.Lockout.MaxFailedAccessAttempts = 10;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                    options.Lockout.AllowedForNewUsers = true;

                    // User settings
                    options.User.RequireUniqueEmail = true;
                }
            );

            //services.ConfigureApplicationCookie(options =>
            //{
            //    //Cookie settings
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.Expiration = TimeSpan.FromDays(150);
            //    //options.LoginPath = "/Account/Login";
            //    //options.LogoutPath = "/Account/Logout";
            //    //options.AccessDeniedPath = "/Account/AccessDenied";
            //    options.SlidingExpiration = true;
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseWelcomePage("/welcome");

            app.Map("/index", CustomIndexHandler);

            //�������� ���� ����������� ���������� ������� (����� TokenMiddlrware)
            app.UseMiddleware<TokenMiddleware>();

            UseSample(app);

            var helloMsg = _configuration["CustomHelloWorld"];
            var logLevel = _configuration["Logging:LogLevel:Microsoft"];

            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                // ������� �� ��������� ������� �� ��� ������ ���������� "/"
                // ������ ������ ����������� ��� �����������,
                // ������ - ��� �������� (������) � �����������,
                // ���� ����� �� ������� - ������������ �������� �� ���������:
                // ��� ����������� ��� "Home",
                // ��� �������� - "Index"
            });

            //�������� ���������� ������ app.Use, �� �� ��������� � �������� ��������� ������� next, �������, ���� � ���� ��������
            //run, �� ����� ���� ������ ��� ������ ������ �� ���������
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello from Run method");
            });
        }

        private void UseSample(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                bool isError = false;
                if (isError)
                {
                    await context.Response.WriteAsync("Hello from method Use method");
                }
                else //����� next �������, � ������ ���������� ������, ������ ������ �� ��������
                {
                    await next.Invoke();
                }
            });
        }

        private void CustomIndexHandler(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello from custom /Index handler");
            });
        }
    }
}
