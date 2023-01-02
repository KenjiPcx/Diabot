// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.18.1

using Diabot.Dialogs.Meals;
using Diabot.Models;
using Diabot.Services.Interfaces;
using DiabotBotService;
using DiabotBotService.CognitiveModels;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiabotBotService.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private readonly ILogger _logger;
        private readonly IMealService _mealService;

        // Dependency injection uses this constructor to instantiate MainDialog
        public MainDialog(AddMealDialog addMealDialog, RemoveMealDialog removeMealDialog, ILogger<MainDialog> logger, IMealService mealService)
            : base(nameof(MainDialog))
        {
            _logger = logger;
            _mealService = mealService;

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(addMealDialog);

            var waterfallSteps = new WaterfallStep[]
            {
                IntroStepAsync,
                ActStepAsync,
                FinalStepAsync,
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }


        private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var msg = "Welcome to Diabot Client. What can I do for you";
            var promptMessage = MessageFactory.Text(msg, msg, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var action = (string)stepContext.Result;
            action = action.Replace(" ", "").ToLowerInvariant();

            switch (action)
            {
                case "addmeal":
                    return await stepContext.BeginDialogAsync(nameof(AddMealDialog), new Meal(), cancellationToken);
                //case "removemeal":
                //    return await stepContext.BeginDialogAsync(nameof(RemoveMealDialog), null, cancellationToken);
                default:
                    var msg = "Command not found, please try again.";
                    var promptMessage = MessageFactory.Text(msg, msg, InputHints.ExpectingInput);
                    return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // If the child dialog ("BookingDialog") was cancelled, the user failed to confirm or if the intent wasn't BookFlight
            // the Result here will be null.
            if (stepContext.Result is Meal result)
            {
                // Now we have all the booking details call the booking service.
                await _mealService.AddMeal(result);

                // If the call to the booking service was successful tell the user.
                var messageText = $"I have added '{result.MealName}' to your saved meals";
                var message = MessageFactory.Text(messageText, messageText, InputHints.IgnoringInput);
                await stepContext.Context.SendActivityAsync(message, cancellationToken);
            }

            // Restart the main dialog with a different message the second time around
            var promptMessage = "What else can I do for you?";
            return await stepContext.ReplaceDialogAsync(InitialDialogId, promptMessage, cancellationToken);
        }
    }
}
