using System;
using System.Net;
using System.Threading.Tasks;
using jmicro1.Adapters;
using Microsoft.Extensions.Logging;
using Network.Core.HTTP;
using Network.Core.HTTP.Serialization.Exceptions;
using Network.HTTP.Serialization;
using SimpleNetFramework.Infrastructure.Middlewares;
using Telegram.Bot.Types;
using TelegramWebApp;
using TelegramWebApp.Extensions;
using ThinServer.TCP;
using HttpListener = Network.HTTP.HttpListener;
using IServer = Network.Core.Server.IServer;
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
        
        // Внедряем Телеграмм бота
        telegramAppBuilder.AddTelegramBot(() => telegramAppBuilder.Configuration["TELEGRAM_BOT_TOKEN"]!);
        
        // Внедряем маршрутизацию
        telegramAppBuilder.AddTeleRouting(config => config.AddFromAssembly(typeof(Program).Assembly));
        
        
        // Собираем приложение
        TelegramWebApplication telegramApp = telegramAppBuilder.Build();

        // Добавляем глобальный отлов ошибок
        telegramApp.UseMiddleware<ExceptionHandlerMiddleware<Update>>();

        // Добавляем маршрутизацию в pipeline
        telegramApp.UseTeleRouting();

        // Запускаем сервер
        await telegramApp.StartAsync();
    }
}