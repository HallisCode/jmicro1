using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using jmicro1.Adapters;
using Microsoft.Extensions.DependencyInjection;
using Network.Core.HTTP;
using Network.Core.HTTP.Serialization.Exceptions;
using Network.Core.Server;
using Network.Core.TCP.TCP;
using Network.HTTP.Serialization;
using Network.TCP;
using Network.ThinServer.Logger;
using Telegram.Bot;
using TelegramWebApp;
using TeleRoute.Core.Routing;
using TeleRoute.Infrastructure.Routing;
using ThinServer.TCP;
using HttpListener = Network.HTTP.HttpListener;

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
        ILogger logger = new Logger();
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
        TelegramBotClient bot = new TelegramBotClient("te");
        telegramAppBuilder.Services.AddSingleton<ITelegramBotClient>(bot);


        // Внедряем маршрутизацию
        telegramAppBuilder.Services.AddSingleton<IRouteTree>(routeTree);


        // Собираем приложение
        TelegramWebApplication telegramApp = telegramAppBuilder.Build();


        // Добавляем маршрутизацию в pipeline
        telegramApp.UseMiddleware<RoutingMiddleware>();

        // Запускаем сервер
        await telegramApp.StartAsync();
    }
}