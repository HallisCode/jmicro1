using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using jmicro1.Adapters;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using TeleRoute.Core.Routing;
using TeleRoute.Infrastructure.Routing;
using TeleRoute.SimpleNetFramework;
using TeleRoute.SimpleNetFramework.Middlewares;
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
        IRouteBuilder routeBuilder = new RouteBuilder();
        routeBuilder.AddFromAssembly(Assembly.GetAssembly(typeof(Program))!);
        IRouteTree routeTree = routeBuilder.Build();

        telegramAppBuilder.Services.AddSingleton<IRouteTree>(routeTree);
        telegramAppBuilder.Services.AddSingleton<IRouteHandler, RouteHandler>();

        // Телеграм бот
        TelegramBotClient bot = new TelegramBotClient("ff");
        telegramAppBuilder.Services.AddSingleton<ITelegramBotClient, TelegramBotClient>();


        // Внедряем маршрутизацию
        telegramAppBuilder.Services.AddSingleton<IRouteTree>(routeTree);

        // Собираем приложение
        TelegramWebApplication telegramApp = telegramAppBuilder.Build();

        // Добавляем маршрутизацию в pipeline
        telegramApp.UseMiddleware<RoutingMiddleware>();

        await telegramApp.StartAsync();
    }
}