using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diabot.Models;
using Diabot.Services.Interfaces;
using Diabot.Views.Scheduler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diabot.ViewModels.Scheduler
{
    [QueryProperty(nameof(MealSession), "MealSession")]
    public partial class ScheduleItemDetailsViewModel : BaseViewModel
    {
        private readonly IMealService _mealService;
        private readonly ISchedulerService _schedulerService;

        public ScheduleItemDetailsViewModel(IMealService mealService, ISchedulerService schedulerService)
        {
            _mealService = mealService;
            _schedulerService = schedulerService;
            Title = "Meal Session Details";
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MealDuration))]
        ScheduleItem mealSession;

        public int MealDuration => MealSession != null ? (MealSession.To - MealSession.From).Minutes : 0;

        [ObservableProperty]
        ObservableCollection<Meal> meals;

        [ObservableProperty]
        ObservableCollection<NutritionMetric> nutritionMetrics;

        [RelayCommand]
        async Task FetchAllMealsAsync()
        {
            if (IsBusy) return;
            if (MealSession == null) return;

            try
            {
                IsBusy = true;
                Meals = await _mealService.GetMealsByIds(MealSession.MealIds);
                AggregateNutrition();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error loading meals", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task GoToEditMealSessionPageAsync()
        {
            if (IsBusy) return;

            await Shell.Current.GoToAsync($"{nameof(EditScheduleItemPage)}", true, new Dictionary<string, object>
            {
                { "MealSession", MealSession }
            });
        }

        [RelayCommand]
        async Task DeleteMealSessionAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                await _schedulerService.DeleteScheduleItem(MealSession.ScheduleItemId.ToString());
                await Shell.Current.GoToAsync($"{nameof(SchedulerPage)}?Reload={true}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error deleting meal session", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        void AggregateNutrition()
        {
            if (Meals == null) return;

            double slowCarbs = 0;
            double mediumCarbs = 0;
            double fastCarbs = 0;
            double extraCarbsOffset = 0;

            foreach (var meal in Meals)
            {
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
                extraCarbsOffset += meal.ExtraCarbsOffset;
            }

            NutritionMetrics = new ObservableCollection<NutritionMetric> {
                new NutritionMetric { Name = "Slow Carbs", Value = slowCarbs },
                new NutritionMetric { Name = "Medium Carbs", Value = mediumCarbs },
                new NutritionMetric { Name = "Fast Carbs", Value = fastCarbs },
                new NutritionMetric { Name = "Extra Carbs Offset", Value = extraCarbsOffset },
            };
        }
    }
}
