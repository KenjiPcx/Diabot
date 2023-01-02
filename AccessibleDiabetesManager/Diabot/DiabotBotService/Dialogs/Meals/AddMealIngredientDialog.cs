using Diabot.Dialogs.Helpers;
using Diabot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiabotBotService.Dialogs
{
    public class AddMealIngredientDialog : CancelAndHelpDialog
    {
        private const string AddIngredientNameStepMsgText = "What is the ingredient's name?";
        private const string SelectIngredientCarbTypeStepMsgText = "What is the ingredient's carb type, fast? slow? or medium?";
        private const string AddIngredientCarbAmountStepMsgText = "What is the ingredient's carb amount?";

        public AddMealIngredientDialog()
            : base(nameof(AddMealIngredientDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new NumberPrompt<double>(nameof(NumberPrompt<double>), CarbAmountPromptValidatorAsync));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));

            var waterfallSteps = new WaterfallStep[]
            {
                    AddIngredientNameStepAsync,
                    SelectIngredientCarbTypeStepAsync,
                    AddIngredientCarbAmountStepAsync,
                    ConfirmStepAsync,
                    FinalStepAsync,
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> AddIngredientNameStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var ingredientDetails = (Ingredient)stepContext.Options;

            if (ingredientDetails.IngredientName == null)
            {
                var promptMessage = MessageFactory.Text(AddIngredientNameStepMsgText, AddIngredientNameStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(ingredientDetails.IngredientName, cancellationToken);
        }

        private async Task<DialogTurnResult> SelectIngredientCarbTypeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var ingredientDetails = (Ingredient)stepContext.Options;

            ingredientDetails.IngredientName = (string)stepContext.Result;
            
            if (ingredientDetails.CarbType is CarbType.None)
            {
                List<Choice> carbTypes = Enum.GetValues(typeof(CarbType)).Cast<CarbType>().Where(carbtype => carbtype != CarbType.None).Select(carbtype => new Choice { Value = carbtype.ToString() }).ToList();
                var promptMessage = MessageFactory.Text(SelectIngredientCarbTypeStepMsgText, SelectIngredientCarbTypeStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(ChoicePrompt), new PromptOptions { Prompt = promptMessage, Choices = carbTypes }, cancellationToken);
            }

            return await stepContext.NextAsync(ingredientDetails.CarbType, cancellationToken);
        }

        private async Task<DialogTurnResult> AddIngredientCarbAmountStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var ingredientDetails = (Ingredient)stepContext.Options;

            ingredientDetails.CarbType = (CarbType)Enum.Parse(typeof(CarbType), ((FoundChoice)stepContext.Result).Value);

            var promptMessage = MessageFactory.Text(AddIngredientCarbAmountStepMsgText, AddIngredientCarbAmountStepMsgText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(NumberPrompt<double>), new PromptOptions
            {
                Prompt = promptMessage,
                RetryPrompt = MessageFactory.Text("Value can be decimal and it must be greater than or equal 0")
            }, cancellationToken);
        }

        private async Task<DialogTurnResult> ConfirmStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var ingredientDetails = (Ingredient)stepContext.Options;

            ingredientDetails.CarbAmount = (double)stepContext.Result;

            var messageText = $"Please confirm, the ingredient details: {ingredientDetails.IngredientName} with {ingredientDetails.CarbAmount} of {ingredientDetails.CarbType.ToString().ToLowerInvariant()} . Is this correct?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);

            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result)
            {
                var ingredientDetails = (Ingredient)stepContext.Options;

                return await stepContext.EndDialogAsync(ingredientDetails, cancellationToken);
            }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }

        private static Task<bool> CarbAmountPromptValidatorAsync(PromptValidatorContext<double> promptContext, CancellationToken cancellationToken)
        {
            // This condition is our validation rule. You can also change the value at this point.
            return Task.FromResult(promptContext.Recognized.Succeeded && promptContext.Recognized.Value > 0);
        }
    }
}

