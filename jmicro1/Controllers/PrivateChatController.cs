using System;
using System.Threading.Tasks;
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

        public PrivateChatController(ITelegramBotClient botClient)
        {
            _botClient = botClient;
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
        }

        [TeleRoute]
        [WhiteListFilter(689425288)]
        public async Task HandleWhiteList(Update update)
        {
            await _botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Hello mr.Developer"
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
    }
}