using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebStore.Infrastructure
{
    internal class TokenMiddleware

    {
        private const string correctToken = "qwerty123";

        public RequestDelegate Next { get; }

        public TokenMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        public async Task InvokeAsync (HttpContext context)
        {
            var token = context.Request.Query["referenceToken"];

            //если нет токена, то ничего не делаем, передаём запрос дальше по конвейеру.
            if (string.IsNullOrEmpty(token))
            {
                await Next.Invoke(context);
                return;
            }

            if (token == correctToken)
            {
                //обрабатываем токен... Применяем скидки или какую-то ещё бизнес логику и вызываем следующий метод в конвеере
                // методом next
                await Next.Invoke(context);
            }
            else
            {
                await context.Response.WriteAsync("Token is incorrect");
            }
        }
    }
}
