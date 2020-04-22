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
            //��������� �������, ����������� ��� mvc
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(SimpleActionFilter)); //����������� �� ����
                // �������������� ������� �����������
                //options.Filters.Add(new SimpleActionFilter()); // ����������� �� �������
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

            //�������� ���� ����������� ���������� ������� (����� TokenMiddlrware)
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
