using System;
using System.Threading.Tasks;
using Achievements.Contracts.Input;
using jmicro1.Contracts.Output;
using MassTransit;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TeleRoute.Infrastructure.Routing.Attributes;
using TeleRoute.Infrastructure.Routing.Filters;

namespace jmicro1.TelegramControllers
{
    [TeleRoute]
    [GroupChatFilter]
    public class GroupController
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<GroupController> _logger;
        private readonly IPublishEndpoint _publisher;
        private readonly ISendEndpointProvider _sendEndpoint;

        public GroupController(
            ILogger<GroupController> logger,
            ITelegramBotClient botClient,
            IPublishEndpoint publisher,
            ISendEndpointProvider sendEndpoint
        )
        {
            _botClient = botClient;
            _logger = logger;
            _publisher = publisher;
            _sendEndpoint = sendEndpoint;
        }

        /// <summary>
        /// Отправляет все команды в соответствующие очереди.
        /// </summary>
        /// <param name="update"></param>
        [TeleRoute]
        [IsCommandFilter]
        [AllowedUpdateType(UpdateType.Message)]
        public async Task CatchAllCommands(Update update)
        {
            string command = update.Message.Text.Split(' ')[0];

            switch (command)
            {
                case ("/achievement"):

                    ISendEndpoint endpoint = await _sendEndpoint.GetSendEndpoint(
                        new Uri($"queue:{AchievementJmicro1Queue.QueueName}")
                    );

                    endpoint.Send<UpdateReceived>(new UpdateReceived(
                        update: update,
                        command: command,
                        commandArgs: update.Message.Text.Split(' ')[1..]));

                    break;
            }

            _logger.LogInformation("Command was sent to concrete service!");
        }

        /// <summary>
        /// Отправляет все сообщения, котоыре не попали под другие фильтры в шину.
        /// </summary>
        /// <param name="update"></param>
        [TeleRoute]
        [AllowedUpdateType(UpdateType.Message)]
        public async Task CatchAllMessages(Update update)
        {
            UpdateReceived updateRecieved = new UpdateReceived(update = update);

            await _publisher.Publish<UpdateReceived>(updateRecieved);
        }
    }
}