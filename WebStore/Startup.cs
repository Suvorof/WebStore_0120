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
            //Добавляем сервисы, необходимые для mvc
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(SimpleActionFilter)); //подключение по типу
                // альтернативный вариант подключения
                //options.Filters.Add(new SimpleActionFilter()); // подключение по объекту
            });

            services.AddDbContext<WebStoreContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

            // Добавляем разрешение зависимости.Т.е. каждый раз когда мы встречаем IEmployeesService экз-р в коде, мы возвращаем
            // экз-р InMemoryEmployeesService. Сервис добавлен как SingleTon т.е. создаётся один экземпляр InMemoryEmployeesService
            // на всё приложение и пересоздаваться этот экземпляр не будет в течении жизни экземпляра.
            // Transient означает, что экземпляр InMemoryEmployeesService будет пересоздаваться каждый раз когда к нему идёт обращение
            // Scoped означает, что будет создаваться один экземпляр на один http запрос
            services.AddSingleton<IEmployeesService, InMemoryEmployeesService>();
            services.AddScoped<IProductService, SqlProductService>();
            //services.AddTransient<IEmployeesService, InMemoryEmployeesService>();
            //services.AddScoped<IEmployeesService, InMemoryEmployeesService>();

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<WebStoreContext
                >() //обязательно! связываем identity с entityframework core для создания миграций и таблиц в бд с юзерами
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

                    // Как будем лочить пользователя
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

            //внедрили свой собственный обработчик запроса (класс TokenMiddlrware)
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
                // Маршрут по умолчанию состоит из трёх частей разделённых "/"
                // Первой частью указывается имя контроллера,
                // второй - имя действия (метода) в контроллере,
                // Если часть не указана - используются значения по умолчанию:
                // для контроллера имя "Home",
                // для действия - "Index"
            });

            //работает аналогично методу app.Use, но не принимает в качестве параметра делегат next, поэтому, если в дело вступает
            //run, то после него запрос уже никуда дальше не передаётся
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
                else //метод next передаёт, в случае отсутствия ошибки, запрос дальше на конвеере
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
