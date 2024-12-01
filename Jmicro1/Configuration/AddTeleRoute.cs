using System;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using TeleRoute.Core.Routing;
using TeleRoute.Infrastructure.Routing;

namespace Jmicro1.Configuration
{
    public static class AddTeleRouteExtension
    {
        public static void AddTeleRoute(this IServiceCollection services, Action<IRouteBuilder> options)
        {
            IRouteBuilder routeBuilder = new RouteBuilder();
            options.Invoke(routeBuilder);

            IRouteTree routeTree = routeBuilder.Build();
            services.AddSingleton<IRouteTree>(routeTree);

            services.AddSingleton<IRouteHandler, RouteHandler>();
        }
    }
}