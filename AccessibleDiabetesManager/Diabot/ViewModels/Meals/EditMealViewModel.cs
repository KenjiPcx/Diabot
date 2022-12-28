using Diabot.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diabot.Services.Interfaces;
using Diabot.Views;

namespace Diabot.ViewModels
{
    [QueryProperty(nameof(Meal), "Meal")]
    public partial class EditMealViewModel : BaseViewModel
    {
        private readonly IMealService _mealService;
        public EditMealViewModel(IMealService mealService)
        {
            _mealService = mealService;
            Title = "Edit Meal";
        }

        public List<CarbType> CarbTypes => Enum.GetValues(typeof(CarbType)).Cast<CarbType>().ToList();

        [ObservableProperty]
        Meal meal;

        [ObservableProperty]
        string newIngredientName;

        [ObservableProperty]
        CarbType newIngredientCarbType;

        [ObservableProperty]
        double newIngredientCarbAmount;

        [RelayCommand]
        void AddIngredient()
        {
            if (IsBusy) return;
            if (NewIngredientName == "") return;

            IsBusy = true;
            Meal.Ingredients.Add(new Ingredient
            {
                IngredientName = NewIngredientName,
                CarbType = NewIngredientCarbType,
                CarbAmount = NewIngredientCarbAmount
            });

            NewIngredientName = "";
            NewIngredientCarbType = CarbType.Slow;
            NewIngredientCarbAmount = 0;
            IsBusy = false;
        }

        [RelayCommand]
        void RemoveIngredient(Ingredient ingredient)
        {
            if (IsBusy) return;
            if (!Meal.Ingredients.Contains(ingredient)) return;

            IsBusy = true;
            Meal.Ingredients.Remove(ingredient);
            IsBusy = false;
        }

        [RelayCommand]
        async Task SaveMealAsync()
        {
            if (IsBusy) return;
            if (Meal.MealName == "" || Meal.Ingredients.Count == 0)
            {
                await Shell.Current.DisplayAlert("Unable to save meal", "A meal must have a non empty name and must have carb ingredients", "Ok");
                return;
            }

            try
            {
                IsBusy = true;
                await _mealService.UpdateMeal(Meal.MealId.ToString(), Meal);
                await Shell.Current.GoToAsync($"{nameof(MealDetailsPage)}", true, 
                    new Dictionary<string,object>
                    {
                        { "Meal", Meal }
                    });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "Ok");
            }
            finally 
            { 
                IsBusy = true;
            }
        }
    }
}
