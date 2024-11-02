using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using jmicro1.Adapters;
using Microsoft.Extensions.DependencyInjection;
using TelegramWebApplication.Core.Routing;
using TelegramWebApplication.Infrastructure;
using TelegramWebApplication.Infrastructure.Routing;
using TelegramWebApplication.Infrastructure.Routing.Middlewares;
using ThinServer;
using ThinServer.HTTP;
using ThinServer.Logger;

namespace jmicro1;

class Program
{
    static async Task Main(string[] args)
    {
        // Инициализируем настройки к серверу
        IPEndPoint endPoint = IPEndPoint.Parse("0.0.0.0:8080");
        IHttpSerializer serializer = new HttpSerializer();
        ILogger logger = new Logger();

        // Сервер
        IServer server = new ThinServer.ThinServer(endPoint, serializer, logger);
        ServerAdapter serverAdapter = new ServerAdapter(server);

        // Билдер TelegramWebApplication
        TelegramWebApplicationBuilder telegramAppBuilder = new TelegramWebApplicationBuilder();
        telegramAppBuilder.SetServer(serverAdapter);

        // Маршрутизация
        ITelegramRouteBuilder telegramRouteBuilder = new TelegramRouteBuilder();
        telegramRouteBuilder.AddFromAssembly(Assembly.GetAssembly(typeof(Program)));
        ITelegramRouteTree telegramRouteTree = telegramRouteBuilder.Build();

        // Внедряем маршрутизацию
        telegramAppBuilder.Services.AddSingleton<ITelegramRouteTree>(telegramRouteTree);

        // Собираем приложение
        TelegramWebApplication.Infrastructure.TelegramWebApplication
            telegramWebApplication = telegramAppBuilder.Build();

        telegramWebApplication.UseMiddleware<RoutingMiddleware>();

        await telegramWebApplication.StartAsync();
    }
}