using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diabot.Models;
using Diabot.Services.Interfaces;
using Diabot.Views.Meals;
using System.Collections.ObjectModel;

namespace Diabot.ViewModels.Meals
{
    public partial class AddMealViewModel : BaseViewModel
    {
        private readonly IMealService _mealService;

        public AddMealViewModel(IMealService mealService)
        {
            _mealService = mealService;
            Title = "New Meal";
        }

        public List<CarbType> CarbTypes => Enum.GetValues(typeof(CarbType)).Cast<CarbType>().ToList();

        [ObservableProperty]
        string mealName;

        [ObservableProperty]
        string mealDescription;

        [ObservableProperty]
        string mealImageUrl;

        [ObservableProperty]
        ObservableCollection<Ingredient> ingredients = new();

        [ObservableProperty]
        string newIngredientName;

        [ObservableProperty]
        CarbType newIngredientCarbType;

        [ObservableProperty]
        double newIngredientCarbAmount;

        [ObservableProperty]
        double extraCarbsOffset;

        [RelayCommand]
        void AddIngredient()
        {
            if (IsBusy) return;
            if (NewIngredientName == "") return;

            IsBusy = true;
            Ingredients.Add(new Ingredient
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
            if (!Ingredients.Contains(ingredient)) return;

            IsBusy = true;
            Ingredients.Remove(ingredient);
            IsBusy = false;
        }

        [RelayCommand]
        async Task AddMealAsync()
        {
            if (IsBusy) return;
            if (MealName == "" || Ingredients.Count == 0)
            {
                await Shell.Current.DisplayAlert("Unable to add meal", "A meal must have a non empty name and must have carb ingredients", "Ok");
                return;
            }

            try
            {
                IsBusy = true;
                Meal newMeal = new()
                {
                    MealName = MealName,
                    MealDescription = MealDescription,
                    ImageUrl = MealImageUrl,
                    Ingredients = Ingredients,
                    ExtraCarbsOffset = ExtraCarbsOffset
                };
                await _mealService.AddMeal(newMeal);

                MealName = "";
                MealDescription = "";
                MealImageUrl = "";
                Ingredients.Clear();
                ExtraCarbsOffset = 0;
                await Shell.Current.GoToAsync($"{nameof(MealsPage)}?Reload={true}", true);
            }
            catch (Exception ex) 
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "Ok");
            }
            finally 
            {
                IsBusy = false;
            }
        }
    }
}
