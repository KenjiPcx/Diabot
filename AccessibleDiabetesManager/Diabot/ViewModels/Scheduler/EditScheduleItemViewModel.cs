using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diabot.Models;
using Diabot.Services.Interfaces;
using System.Collections.ObjectModel;

namespace Diabot.ViewModels.Scheduler
{
    public partial class EditScheduleItemViewModel : BaseViewModel
    {
        private readonly IMealService _mealService;
        private readonly ISchedulerService _schedulerService;

        public EditScheduleItemViewModel(IMealService mealService, ISchedulerService schedulerService)
        {
            _mealService = mealService;
            _schedulerService = schedulerService;
            Title = "Edit a meal session";
            Task.Run(LoadMealPickerOptions);
        }

        [ObservableProperty]
        ScheduleItem mealSession;

        [ObservableProperty]
        Meal selectedMeal;
        
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


        public ObservableCollection<Meal> MealPickerOptions { get; set; }

        [ObservableProperty]
        bool isLoadingMealPickerOptions;

        private async Task LoadMealPickerOptions()
        {
            if (IsLoadingMealPickerOptions) return;

            try
            {
                IsLoadingMealPickerOptions = true;
                MealPickerOptions = await _mealService.GetAllMeals();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Unable to fetch meals", ex.Message, "Ok");
            }
            finally
            {
                IsLoadingMealPickerOptions = false;
            }
        }

        [RelayCommand]
        void AddMealToScheduleItemAsync()
        {
            Meals.Add(selectedMeal);
        }

        [RelayCommand]
        void RemoveMealFromScheduleItemAsync()
        {
            Meals.Remove(selectedMeal);
        }

        [RelayCommand]
        async Task UpdateMealSessionAsync()
        {
            if (IsBusy) return;
            
            try
            {
                IsBusy = true;
                await _schedulerService.UpdateScheduleItem(MealSession.ScheduleItemId.ToString(), MealSession);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error updating meal session", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
