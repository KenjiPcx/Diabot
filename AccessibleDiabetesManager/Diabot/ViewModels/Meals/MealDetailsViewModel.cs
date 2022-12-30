using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diabot.Models;
using Diabot.Services.Interfaces;
using Diabot.Views.Meals;
using System.Collections.ObjectModel;

namespace Diabot.ViewModels.Meals
{
    [QueryProperty(nameof(Meal), "Meal")]
    public partial class MealDetailsViewModel : BaseViewModel
    {
        private readonly IMealService _mealService;
        public MealDetailsViewModel(IMealService mealService)
        {
            _mealService = mealService;
        }

        [ObservableProperty]
        Meal meal;

        [RelayCommand]
        async Task GoToEditPageAsync(Meal meal)
        {
            if (IsBusy) return;
            if (meal is null) return;

            await Shell.Current.GoToAsync($"{nameof(EditMealPage)}", true, new Dictionary<string, object>
            {
                { "Meal", meal }
            });
        }

        [RelayCommand]
        async Task DeleteMealAsync(Meal meal)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                bool delete = await Shell.Current.DisplayAlert($"Remove {Meal.MealName}?", $"Are you sure you want to remove {Meal.MealName}? This action cannot be undone.", "Confirm", "Back");
                
                if (!delete) return;
                await _mealService.DeleteMeal(meal.MealId.ToString());
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

        [ObservableProperty]
        ObservableCollection<NutritionMetric> nutritionMetrics;

        [RelayCommand]
        void AggregateNutrition()
        {
            if (Meal is null) return;

            double slowCarbs = 0;
            double mediumCarbs = 0;
            double fastCarbs = 0;

            foreach (var ingredient in meal.Ingredients)
            {
                if (ingredient.CarbType == CarbType.Slow)
                {
                    slowCarbs += ingredient.CarbAmount;
                }
                else if (ingredient.CarbType == CarbType.Medium)
                {
                    mediumCarbs += ingredient.CarbAmount;
                }
                else
                {
                    fastCarbs += ingredient.CarbAmount;
                }
            }

            NutritionMetrics = new ObservableCollection<NutritionMetric> {
                new NutritionMetric { Name = "Slow Carbs", Value = slowCarbs },
                new NutritionMetric { Name = "Medium Carbs", Value = mediumCarbs },
                new NutritionMetric { Name = "Fast Carbs", Value = fastCarbs },
            };
        }
    }
}
