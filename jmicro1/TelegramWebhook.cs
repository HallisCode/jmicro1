using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using TeleRoute.Core.Routing;

namespace jmicro1
{
    [ApiController]
    [Route("telegram/webhook")]
    public class TelegramWebhookController : ControllerBase
    {
        private readonly IRouteHandler _routeHandler;

        private readonly ILogger<TelegramWebhookController> _logger;


        public TelegramWebhookController(
            IRouteHandler routeHandler,
            ILogger<TelegramWebhookController> logger
        )
        {
            _routeHandler = routeHandler;
            _logger = logger;
        }

        [HttpPost]
        public async Task Handle(Update update)
        {
            await _routeHandler.Handle(update);
        }
    }
}