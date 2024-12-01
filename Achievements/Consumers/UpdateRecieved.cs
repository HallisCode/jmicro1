using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Achievements.Contracts.Output;
using Achievements.Models;
using Achievements.Services.Abstractions;
using Jmicro1.Contracts.Input;
using Jmicro1.Contracts.Output;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Achievements.Consumers
{
    public class UpdateRecievedConsumer : IConsumer<UpdateReceived>
    {
        private readonly IUserChatStatsService _userChatStatsService;
        private readonly IAchievementService _achievementService;

        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IPublishEndpoint _publishEndpoint;

        private readonly ILogger<UpdateRecievedConsumer> _logger;


        public UpdateRecievedConsumer(
            ILogger<UpdateRecievedConsumer> logger,
            IUserChatStatsService userChatStatsService,
            IAchievementService achievementService,
            ISendEndpointProvider sendEndpointProvider,
            IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _userChatStatsService = userChatStatsService;
            _achievementService = achievementService;
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<UpdateReceived> context)
        {
            if (context.Message.IsCommand)
            {
                if (context.Message.CommandArgs.Length < 1)
                {
                    // TODO Exception handling
                    return;
                }

                await HandleCommandAsync(context);
            }
            else
            {
                await HandleCommonUpdateAsync(context);
            }
        }

        private async Task HandleCommandAsync(ConsumeContext<UpdateReceived> context)
        {
            if (context.Message.IsCommand is false)
            {
                throw new Exception("Запрос не является командой!");
            }


            ISendEndpoint endpoint = await _sendEndpointProvider.GetSendEndpoint(
                new Uri($"queue:{Jmicro1AchievementQueue.QueueName}")
            );

            switch (context.Message.CommandArgs[0].ToLower())
            {
                case ("list"):
                    // Внутреннее представление
                    Achievement[] achievements =
                        await _achievementService.SelectAllAchievementsAsync(context.Message.Update.Message!.Chat.Id);

                    // Внешнее представление
                    List<Contracts.Models.Achievement> _achievements =
                        new List<Contracts.Models.Achievement>();

                    // Преобразуем из внутренней модели во внешнюю 
                    foreach (var achievement in achievements)
                    {
                        _achievements.Add(new Contracts.Models.Achievement(
                            chatId: achievement.ChatId,
                            title: achievement.Title,
                            messages: achievement.Messages)
                        );
                    }

                    DisplayAchievementList displayAchievementList =
                        new DisplayAchievementList(_achievements, context.Message.Update);

                    await endpoint.Send<DisplayAchievementList>(displayAchievementList);

                    break;

                case ("add"):
                    if (context.Message.CommandArgs.Length < 3)
                    {
                        // TODO Exception handling
                        return;
                    }

                    await _achievementService.CreateAchievementAsync(
                        chatId: context.Message.Update.Message!.Chat.Id,
                        title: context.Message.CommandArgs[1],
                        messages: long.Parse(context.Message.CommandArgs[2]));
                    break;

                case ("del"):
                case ("delete"):
                case ("remove"):
                case ("rm"):

                    if (context.Message.CommandArgs.Length < 2)
                    {
                        // TODO Exception handling
                        return;
                    }

                    await _achievementService.DeleteAchievementAsync(
                        chatId: context.Message.Update.Message!.Chat.Id,
                        title: context.Message.CommandArgs[1]
                    );
                    break;
            }
        }

        private async Task HandleCommonUpdateAsync(ConsumeContext<UpdateReceived> context)
        {
            if (context.Message.IsCommand is true)
            {
                throw new Exception("Запрос является командой!");
            }

            var userId = context.Message.Update.Message!.From!.Id;
            var chatId = context.Message.Update.Message.Chat.Id;

            UserChatStats userChatStats = await _userChatStatsService.IncrementMessageAsync(
                userId: userId,
                chatId: chatId
            );

            Achievement? achievement = await _achievementService.HasBeenAchievedAsync(userChatStats);

            if (achievement is not null)
            {
                await _publishEndpoint.Publish<AchievementAwarded>(new AchievementAwarded(
                    update: context.Message.Update,
                    title: achievement.Title,
                    messages: achievement.Messages)
                );
            }
        }
    }
}