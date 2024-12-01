using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Achievements.Contracts.Models;
using Achievements.Contracts.Output;
using Jmicro1.TelegramBotClientExtension;
using MassTransit;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Jmicro1.Consumers
{
    public class DisplayAchievementListConsumer : IConsumer<DisplayAchievementList>
    {
        private readonly ITelegramBotClient _telegramBot;

        private readonly ILogger<DisplayAchievementListConsumer> _logger;

        public DisplayAchievementListConsumer(ITelegramBotClient telegramBot,
            ILogger<DisplayAchievementListConsumer> logger)
        {
            _telegramBot = telegramBot;

            _logger = logger;
        }

        public async Task Consume(ConsumeContext<DisplayAchievementList> context)
        {
            long chatId = context.Message.Update.Message!.Chat.Id;
            long messageId = context.Message.Update.Message.MessageId;

            Achievement[] achievements = context.Message.Achievements.OrderBy(ach => ach.Messages).ToArray();

            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.AppendLine($"Список достижений:\n");

            foreach (Achievement achievement in achievements)
            {
                messageBuilder.AppendLine($"'{achievement.Title}' — {achievement.Messages} сообщений.");
            }

            _telegramBot.SendTemporaryMessageAsync(
                chatId: chatId,
                text: messageBuilder.ToString(),
                replyParameters: new ReplyParameters()
                {
                    AllowSendingWithoutReply = true,
                    ChatId = chatId,
                    MessageId = (int)messageId
                },
                seconds: 32);
        }
    }
}