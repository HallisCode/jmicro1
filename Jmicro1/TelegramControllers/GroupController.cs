using System;
using System.Linq;
using System.Threading.Tasks;
using Achievements.Contracts.Input;
using Jmicro1.Authorization;
using Jmicro1.Contracts.Output;
using Jmicro1.TelegramBotClientExtension;
using MassTransit;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TeleRoute.Infrastructure.Routing.Attributes;
using TeleRoute.Infrastructure.Routing.Filters;

namespace Jmicro1.TelegramControllers
{
    [TeleRoute]
    [GroupChatFilter]
    public class GroupController
    {
        private readonly ITelegramBotClient _telegramBot;
        private readonly ILogger<GroupController> _logger;
        private readonly IPublishEndpoint _publisher;
        private readonly ISendEndpointProvider _sendEndpoint;

        public GroupController(
            ILogger<GroupController> logger,
            ITelegramBotClient telegramBot,
            IPublishEndpoint publisher,
            ISendEndpointProvider sendEndpoint
        )
        {
            _telegramBot = telegramBot;
            _logger = logger;
            _publisher = publisher;
            _sendEndpoint = sendEndpoint;
        }

        /// <summary>
        /// Отправляет все сообщения, которе не попали под другие фильтры в шину.
        /// </summary>
        /// <param name="update"></param>
        [TeleRoute]
        [AllowedUpdateType(UpdateType.Message)]
        public async Task CatchAllMessages(Update update)
        {
            UpdateReceived updateRecieved = new UpdateReceived(update = update);

            await _publisher.Publish<UpdateReceived>(updateRecieved);
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
            bool isAdmin = await TelegramAuthorization.CheckAdminRights(update, _telegramBot);

            switch (command)
            {
                case ("/achv"):
                case ("/achievement"):

                    if (!isAdmin) return;

                    ISendEndpoint endpoint = await _sendEndpoint.GetSendEndpoint(
                        new Uri($"queue:{AchievementJmicro1Queue.QueueName}")
                    );

                    await endpoint.Send<UpdateReceived>(new UpdateReceived(
                        update: update,
                        command: command,
                        commandArgs: update.Message.Text.Split(' ')[1..]));

                    break;
            }

            _telegramBot.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
        }

        [TeleRoute]
        [IsCommandFilter("help")]
        [AllowedUpdateType(UpdateType.Message)]
        public async Task HandleHelpCommand(Update update)
        {
            bool isAdmin = await TelegramAuthorization.CheckAdminRights(update, _telegramBot);
            if (!isAdmin) return;

            long chatId = update.Message!.Chat.Id;
            long messageId = update.Message.MessageId;

            string message = "Список доступных сервисов:\n\n" +
                             "<b>Achievement:</b>\n" +
                             "/achievement add &lt;title_name&gt; &lt;messages_count&gt;\n" +
                             "/achievement rm &lt;title_name&gt;\n" +
                             "/achievement list";

            _telegramBot.AnswerTemporaryOnCommandAsync(
                command: update.Message,
                text: message,
                parseMode: ParseMode.Html,
                seconds: 32
            );
        }
    }
}