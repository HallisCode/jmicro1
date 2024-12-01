using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Jmicro1.TelegramBotClientExtension
{
    public static class TelegramBotClientExtension
    {
        public static async Task AnswerTemporaryOnCommandAsync(
            this ITelegramBotClient telegramBot,
            Message command,
            string text,
            int seconds,
            ReplyParameters replyParameters = null,
            ParseMode parseMode = ParseMode.None
        )
        {
            Message answer = await telegramBot.SendTextMessageAsync(
                chatId: command.Chat.Id,
                text: text,
                parseMode: parseMode,
                replyParameters: replyParameters
            );

            await Task.Delay(seconds * 1000);

            await telegramBot.DeleteMessageAsync(command.Chat.Id, command.MessageId);

            await telegramBot.DeleteMessageAsync(command.Chat.Id, answer.MessageId);
        }

        public static async Task SendTemporaryMessageAsync(
            this ITelegramBotClient telegramBot,
            long chatId,
            string text,
            int seconds,
            ReplyParameters replyParameters = null,
            ParseMode parseMode = ParseMode.None
        )
        {
            Message answer = await telegramBot.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                parseMode: parseMode,
                replyParameters: replyParameters
            );

            await Task.Delay(seconds * 1000);
            
            await telegramBot.DeleteMessageAsync(chatId, answer.MessageId);
        }
    }
}