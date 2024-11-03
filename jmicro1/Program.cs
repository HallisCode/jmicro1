using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using jmicro1.Adapters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Network.Core.HTTP;
using Network.Core.HTTP.Serialization.Exceptions;
using Network.HTTP.Serialization;
using SimpleNetFramework.Infrastructure.Middlewares;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramWebApp;
using TeleRoute.Core.Routing;
using ThinServer.TCP;
using HttpListener = Network.HTTP.HttpListener;
using IRouteBuilder = TeleRoute.Core.Routing.IRouteBuilder;
using IRouteHandler = TeleRoute.Core.Routing.IRouteHandler;
using IServer = Network.Core.Server.IServer;
using RouteBuilder = TeleRoute.Infrastructure.Routing.RouteBuilder;
using RouteHandler = TeleRoute.Infrastructure.Routing.RouteHandler;
using TcpListener = Network.TCP.TcpListener;

namespace jmicro1;

class Program
{
    static async Task Main(string[] args)
    {
        // Инициализируем TcpListener
        IPEndPoint endPoint = IPEndPoint.Parse("0.0.0.0:8080");
        ITcpListener tcpListener = new TcpListener(endPoint);


        // Инициализируем HttpListener
        IHttpSerializer httpSerializer = new HttpSerializer();
        IHttpListener httpListener = new HttpListener(httpSerializer, tcpListener);


        // Инициализируем сервер
        ILogger<Network.ThinServer.ThinServer> logger = LoggerFactory.Create(
            configure => configure.AddConsole()
        ).CreateLogger<Network.ThinServer.ThinServer>();

        IServer server = new Network.ThinServer.ThinServer(httpListener, logger);
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
        TelegramBotClient bot = new TelegramBotClient("f");
        telegramAppBuilder.Services.AddSingleton<ITelegramBotClient>(bot);


        // Внедряем маршрутизацию
        telegramAppBuilder.Services.AddSingleton<IRouteTree>(routeTree);


        // Собираем приложение
        TelegramWebApplication telegramApp = telegramAppBuilder.Build();

        // Добавляем глобальный отлов ошибок
        telegramApp.UseMiddleware<ExceptionHandlerMiddleware<Update>>();

        // Добавляем маршрутизацию в pipeline
        telegramApp.UseMiddleware<RoutingMiddleware>();

        // Запускаем сервер
        await telegramApp.StartAsync();
    }
}