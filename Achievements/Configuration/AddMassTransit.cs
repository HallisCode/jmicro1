using Achievements.Consumers;
using Achievements.Contracts.Input;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Achievements.Configuration
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
                x.AddConsumer<UpdateRecievedConsumer>();

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

                    configRabbitMQ.ReceiveEndpoint(AchievementJmicro1Queue.QueueName,
                        config =>
                        {
                            config.Consumer<UpdateRecievedConsumer>(context);
                        });
                });
            });
        }
    }
}