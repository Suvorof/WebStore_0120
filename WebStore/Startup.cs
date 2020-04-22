using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.Infrastructure;

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
