using Jmicro1.Consumers;
using Jmicro1.Contracts.Input;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Jmicro1.Configuration
{
    public static class AddConfiguredMassTransitExtension
    {
        public static void AddConfiguredMassTransit(
            this IServiceCollection services,
            string rabbitMqHost,
            string rabbitMqUsername,
            string rabbitMqPassword
        )
        {
            services.AddMassTransit((IBusRegistrationConfigurator x) =>
            {
                x.AddConsumer<AchievementAwardedConsumer>();
                x.AddConsumer<DisplayAchievementListConsumer>();

                x.UsingRabbitMq((context, configRabbitMQ) =>
                {
                    // Configure connect settings
                    configRabbitMQ.Host(
                        host: rabbitMqHost,
                        configure: config =>
                        {
                            config.Username(rabbitMqUsername);
                            config.Password(rabbitMqPassword);
                        }
                    );

                    configRabbitMQ.ReceiveEndpoint(Jmicro1AchievementQueue.QueueName,
                        config =>
                        {
                            config.Consumer<AchievementAwardedConsumer>(context);
                            config.Consumer<DisplayAchievementListConsumer>(context);
                        });
                });
            });
        }
    }
}