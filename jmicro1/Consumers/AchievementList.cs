using System;
using System.Text;
using System.Threading.Tasks;
using Achievements.Contracts.Models;
using Achievements.Contracts.Output;
using MassTransit;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace jmicro1.Consumers
{
    public class GiveOutAchievementListConsumer : IConsumer<GiveOutAchievementList>
    {
        private readonly ITelegramBotClient _telegramBot;

        private readonly ILogger<GiveOutAchievementListConsumer> _logger;

        public GiveOutAchievementListConsumer(ITelegramBotClient telegramBot,
            ILogger<GiveOutAchievementListConsumer> logger)
        {
            _telegramBot = telegramBot;

            _logger = logger;
        }

        public async Task Consume(ConsumeContext<GiveOutAchievementList> context)
        {
            long chatId = context.Message.Update.Message!.Chat.Id;
            long messageId = context.Message.Update.Message.MessageId;

            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.AppendLine($"Список достижений:\n");

            foreach (Achievement achievement in context.Message.Achievements)
            {
                messageBuilder.AppendLine($"'{achievement.Title}' — {achievement.Messages} сообщений.");
            }

            await _telegramBot.SendTextMessageAsync(
                chatId: chatId,
                text: messageBuilder.ToString(),
                replyParameters: new ReplyParameters()
                {
                    AllowSendingWithoutReply = true,
                    ChatId = chatId,
                    MessageId = (int)messageId
                });
        }
    }
}