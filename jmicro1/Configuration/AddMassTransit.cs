using Achievements.Contracts.Models.Output;
using Achievements.Contracts.Output;
using jmicro1.Consumers;
using jmicro1.Contracts;
using jmicro1.Contracts.Input;
using jmicro1.Contracts.Output;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace jmicro1.Configuration
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
                x.AddConsumer<GiveOutAchievementListConsumer>();

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
                            config.Consumer<GiveOutAchievementListConsumer>(context);
                        });
                });
            });
        }
    }
}