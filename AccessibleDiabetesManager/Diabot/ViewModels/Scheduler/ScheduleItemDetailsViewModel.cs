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
        ScheduleItem mealSession;

        [ObservableProperty]
        ObservableCollection<Meal> meals; 

        [ObservableProperty]
        bool isLoadingMeals;

        [RelayCommand]
        async Task GetMealsAsync()
        {
            if (IsLoadingMeals) return;

            try
            {
                IsLoadingMeals = true;
                Meals = await _mealService.GetMealsByIds(MealSession.MealIds);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error loading meals", ex.Message, "Ok");
            }
            finally
            {
                IsLoadingMeals = false;
            }
        }

        [RelayCommand]
        async Task GoToUpdateMealSessionPageAsync()
        {
            if (IsBusy) return;

            await Shell.Current.GoToAsync($"{nameof(EditScheduleItemPage)}", true);
        }

        [RelayCommand]
        async Task DeleteMealSessionAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                await _schedulerService.DeleteScheduleItem(mealSession.ScheduleItemId.ToString());
                await Shell.Current.GoToAsync($"{nameof(SchedulerPage)}");
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
    }
}
