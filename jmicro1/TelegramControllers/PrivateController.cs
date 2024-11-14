using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TeleRoute.Infrastructure.Routing.Attributes;
using TeleRoute.Infrastructure.Routing.Filters;

namespace jmicro1.TelegramControllers
{
    [TeleRoute]
    [PrivateChatFilter]
    [AllowedUpdateType(UpdateType.Message)]
    public class PrivateController
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<PrivateController> _logger;

        public PrivateController(ILogger<PrivateController> logger, ITelegramBotClient botClient)
        {
            _botClient = botClient;
            _logger = logger;
        }

        [TeleRoute]
        public async Task Handle(Update update)
        {
            await _botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: $"\u2699\ufe0fI'm a test bot to learn microservices.\nContact the developer - @HallisPlus.",
                replyParameters: new ReplyParameters()
                {
                    ChatId = update.Message.Chat.Id,
                    MessageId = update.Message.MessageId
                }
            );
        }
    }
}