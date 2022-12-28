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
        [NotifyPropertyChangedFor(nameof(NutritionalInfo))]
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

        private Dictionary<CarbType, double> _carbsDistribution;

        public MealDetailsViewModel()
        {
            _carbsDistribution = new Dictionary<CarbType, double>();
        }

        partial void OnMealChanging(Meal value)
        {
            CalcCarbsDist(value);
        }

        private void CalcCarbsDist(Meal meal)
        {
            _carbsDistribution = new Dictionary<CarbType, double>();
            foreach (var ingredient in meal.Ingredients)
            {
                if (_carbsDistribution.ContainsKey(ingredient.CarbType))
                {
                    _carbsDistribution[ingredient.CarbType] += ingredient.CarbAmount;
                }
                else
                {
                    _carbsDistribution[ingredient.CarbType] = ingredient.CarbAmount;
                }
            }
        }

        public ObservableCollection<TextCell> NutritionalInfo
        {
            get
            {
                try
                {
                    var cells = _carbsDistribution.Select(
                    pair => new TextCell
                    {
                        Text = $"Total {pair.Key} Carbs",
                        Detail = $"{pair.Value}g"
                    });
                    cells = cells.Append(new TextCell
                    {
                        Text = "Extra Carbs Offset",
                        Detail = $"{Meal.ExtraCarbsOffset}g"
                    });
                    return new ObservableCollection<TextCell>(cells);
                } 
                catch (Exception ex)
                {
                    return new ObservableCollection<TextCell>();
                }
            }
        } 
    }
}
