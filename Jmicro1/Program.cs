using System.Threading.Tasks;
using Jmicro1.Middlewares;
using Microsoft.Extensions.Configuration;
using Telegram.Bot.Types;
using Jmicro1.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace Jmicro1;

class Program
{
    static async Task Main(string[] args)
    {
        // Билдер TelegramWebApplication
        WebApplicationBuilder webAppBuilder = WebApplication.CreateBuilder();


        // Configuration
        webAppBuilder.Configuration.AddJsonFile("settings.json");
        webAppBuilder.Configuration.AddEnvironmentVariables();

        // DI: logic asp.net routing
        webAppBuilder.Services.AddMvcCore();

        // DI: telegram client
        TelegramBotClient telegramBotClient = new TelegramBotClient(webAppBuilder.Configuration["TELEGRAM_BOT_TOKEN"]!);
        webAppBuilder.Services.AddSingleton<ITelegramBotClient>(telegramBotClient);
        webAppBuilder.Services.ConfigureTelegramBotMvc();

        // DI: TeleRoute
        webAppBuilder.Services.AddTeleRoute(opt => { opt.AddFromAssembly(typeof(Program).Assembly); }
        );

        // DI: logic services


        // DI: MassTransit (RabbitMQ)
        webAppBuilder.Services.AddConfiguredMassTransit(
            rabbitMqHost: webAppBuilder.Configuration["RabbitMQ:host"]!,
            rabbitMqUsername: webAppBuilder.Configuration["RabbitMQ:username"]!,
            rabbitMqPassword: webAppBuilder.Configuration["RabbitMQ:password"]!
        );


        // Собираем приложение
        WebApplication webApp = webAppBuilder.Build();

        // Middlewares
        webApp.UseMiddleware<ExceptionMiddleware>();

        // Asp.net routing
        webApp.MapControllers();

        // Запускаем сервер
        webApp.Run();
    }
}