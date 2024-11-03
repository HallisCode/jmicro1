using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TeleRoute.Infrastructure.Routing.Attributes;
using TeleRoute.Infrastructure.Routing.Filters;

namespace jmicro1.Controllers
{
    [TeleRoute]
    [AllowedUpdateType(UpdateType.Message)]
    [PrivateChatFilter]
    public class PrivateChatController
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<PrivateChatController> _logger;

        public PrivateChatController(ILogger<PrivateChatController> logger, ITelegramBotClient botClient)
        {
            _botClient = botClient;
            _logger = logger;
        }

        [TeleRoute]
        public async Task Handle(Update update)
        {
            await _botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: $"My datetime is {DateTime.Now}",
                replyParameters: new ReplyParameters()
                {
                    ChatId = update.Message.Chat.Id,
                    MessageId = update.Message.MessageId
                }
            );

            _logger.LogInformation($"New message from : " +
                                   $"\n{update.Message.From.FirstName} {update.Message.From.LastName}" +
                                   $"\n\tid {update.Message.From.Id}"
            );
        }

        [TeleRoute]
        [WhiteListFilter(689425288)]
        public async Task HandleWhiteList(Update update)
        {
            await _botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Hello Mr.Developer"
            );
        }

        [TeleRoute]
        [IsCommandFilter("sudo")]
        public async Task HandleSuperUser(Update update)
        {
            await _botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: $"su",
                replyParameters: new ReplyParameters()
                {
                    ChatId = update.Message.Chat.Id,
                    MessageId = update.Message.MessageId
                }
            );
        }

        [TeleRoute]
        [IsCommandFilter("sudo")]
        [WhiteListFilter(689425288)]
        public async Task HandleWhiteListSuperUser(Update update)
        {
            await _botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: $"already su",
                replyParameters: new ReplyParameters()
                {
                    ChatId = update.Message.Chat.Id,
                    MessageId = update.Message.MessageId
                }
            );
        }
    }
}