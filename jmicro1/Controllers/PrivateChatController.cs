using System.Threading.Tasks;
using Telegram.Bot.Types;
using TeleRoute.Infrastructure.Routing.Attributes;
using TeleRoute.Infrastructure.Routing.Filters;


namespace jmicro1.Controllers
{
    [TeleRoute]
    [PrivateChatFilter]
    public class PrivateChatController
    {
        public PrivateChatController()
        {
        }

        [TeleRoute]
        public async Task Handle(Update update)
        {

        }
    }
}