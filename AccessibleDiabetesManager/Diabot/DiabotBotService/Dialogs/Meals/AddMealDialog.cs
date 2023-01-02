using Diabot.Dialogs.Helpers;
using Diabot.Models;
using DiabotBotService;
using DiabotBotService.Dialogs;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Diabot.Dialogs.Meals
{
    public class AddMealDialog : CancelAndHelpDialog
    {
        public AddMealDialog() : base(nameof(AddMealDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new NumberPrompt<double>(nameof(NumberPrompt<double>)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new AddMealIngredientDialog());

            var waterfallSteps = new WaterfallStep[]
            {
                AddMealNameStepAsync,
                AddMealDescriptionStepAsync,
                AskToAddMealIngredientStepAsync,
                AddMealIngredientStepAsync,
                ConfirmAddMealIngredientStepAsync,
                AddMealExtraCarbOffsetStepAsync,
                ConfirmStepAsync,
                FinalStepAsync,
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private static bool IsAmbiguous(string timex)
        {
            var timexProperty = new TimexProperty(timex);
            return !timexProperty.Types.Contains(Constants.TimexTypes.Definite);
        }

        private async Task<DialogTurnResult> AddMealNameStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var mealDetails = (Meal)stepContext.Options;

            if (mealDetails.MealName == null)
            {
                var msg = "What do you want to name this meal?";
                var promptMessage = MessageFactory.Text(msg, msg, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(mealDetails.MealName, cancellationToken);
        }

        private async Task<DialogTurnResult> AddMealDescriptionStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var mealDetails = (Meal)stepContext.Options;

            mealDetails.MealName = (string)stepContext.Result;

            if (mealDetails.MealDescription == null)
            {
                var msg = "What description do you want for this meal";
                var promptMessage = MessageFactory.Text(msg, msg, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(mealDetails.MealDescription, cancellationToken);
        }

        private async Task<DialogTurnResult> AskToAddMealIngredientStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var mealDetails = (Meal)stepContext.Options;

            mealDetails.MealDescription = (string)stepContext.Result;

            var msg = "Add an ingredient?";
            var promptMessage = MessageFactory.Text(msg, msg, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> AddMealIngredientStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var mealDetails = (Meal)stepContext.Options;

            if ((bool)stepContext.Result)
            {
                var messageText = $"Alright, getting ready to add an ingredient";
                var message = MessageFactory.Text(messageText, messageText, InputHints.IgnoringInput);
                await stepContext.Context.SendActivityAsync(message, cancellationToken);
                return await stepContext.BeginDialogAsync(nameof(AddMealIngredientDialog), new Ingredient(), cancellationToken);
            }

            return await stepContext.NextAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> ConfirmAddMealIngredientStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var mealDetails = (Meal)stepContext.Options;

            mealDetails.Ingredients ??= new();

            if (stepContext.Result is Ingredient result)
            {
                mealDetails.Ingredients.Add(result);

                var messageText = $"I have added the ingredient {result.IngredientName} to the meal";
                var message = MessageFactory.Text(messageText, messageText, InputHints.IgnoringInput);
                await stepContext.Context.SendActivityAsync(message, cancellationToken);
            }

            return await stepContext.NextAsync(mealDetails.Ingredients, cancellationToken);
        }

        private async Task<DialogTurnResult> AddMealExtraCarbOffsetStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var mealDetails = (Meal)stepContext.Options;

            mealDetails.Ingredients = (ObservableCollection<Ingredient>)stepContext.Result;

            var msg = "Add meal extra carb offset";
            var promptMessage = MessageFactory.Text(msg, msg, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(NumberPrompt<double>), new PromptOptions { 
                Prompt = promptMessage,
                RetryPrompt = MessageFactory.Text("Number can be a decimal, but it must be more than 0.")
            }, cancellationToken);
        }

        private async Task<DialogTurnResult> ConfirmStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var mealDetails = (Meal)stepContext.Options;

            mealDetails.ExtraCarbsOffset = (double)stepContext.Result;

            var messageText = $"Please confirm, is this the meal you want to add?\n Name: {mealDetails.MealName} \n Description: {mealDetails.MealDescription} \n Ingredients: {mealDetails.Ingredients[0]}. \n Extra Carbs Offset: {mealDetails.ExtraCarbsOffset}. Is this correct?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);

            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result)
            {
                var mealDetails = (Meal)stepContext.Options;

                return await stepContext.EndDialogAsync(mealDetails, cancellationToken);
            }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }

    }
}
