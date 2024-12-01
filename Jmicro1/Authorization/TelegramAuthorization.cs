using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Jmicro1.Authorization
{
    public static class TelegramAuthorization
    {
        public static async Task<bool> CheckAdminRights(Update update, ITelegramBotClient telegramBot)
        {
            ChatMember[] administrators = await telegramBot.GetChatAdministratorsAsync(update.Message.Chat.Id);

            bool isAdmin = administrators.Any(admin => admin.User.Id == update.Message.From.Id);

            return isAdmin;
        }
    }
}