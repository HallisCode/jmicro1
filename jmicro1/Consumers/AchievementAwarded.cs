using System.Threading.Tasks;
using Achievements.Contracts.Models.Output;
using Achievements.Contracts.Output;
using MassTransit;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace jmicro1.Consumers
{
    public class AchievementAwardedConsumer : IConsumer<AchievementAwarded>
    {
        private readonly ITelegramBotClient _telegramBot;

        private readonly ILogger<AchievementAwardedConsumer> _logger;

        public AchievementAwardedConsumer(ITelegramBotClient telegramBot, ILogger<AchievementAwardedConsumer> logger)
        {
            _telegramBot = telegramBot;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AchievementAwarded> context)
        {
            long chatId = context.Message.Update.Message!.Chat.Id;
            long messageId = context.Message.Update.Message.MessageId;

            string message = $"Поздравляем @{context.Message.Update.Message.From!.Username}, " +
                             $"с получением титула '{context.Message.Title}' за {context.Message.Messages} написанных сообщений.";

            await _telegramBot.SendTextMessageAsync(
                chatId: chatId,
                text: message,
                replyParameters: new ReplyParameters()
                {
                    AllowSendingWithoutReply = true,
                    ChatId = chatId,
                    MessageId = (int)messageId
                });
        }
    }
}