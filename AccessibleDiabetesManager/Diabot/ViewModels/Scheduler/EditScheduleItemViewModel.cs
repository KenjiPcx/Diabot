using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diabot.Models;
using Diabot.Services.Interfaces;
using Diabot.Views.Scheduler;
using System.Collections.ObjectModel;

namespace Diabot.ViewModels.Scheduler
{
    [QueryProperty(nameof(MealSession), "MealSession")]
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
        DateTime selectedDate;

        [ObservableProperty]
        TimeSpan selectedTime;

        public DateTime MealStartTime => SelectedDate.Add(SelectedTime);

        [ObservableProperty]
        int mealDuration;
        
        [ObservableProperty]
        ObservableCollection<Meal> meals;

        [RelayCommand]
        async Task FetchAllMealsAsync()
        {
            if (MealSession == null) return;

            try
            {
                IsBusy = true;
                Meals = await _mealService.GetMealsByIds(MealSession.MealIds);
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

        [ObservableProperty]
        ObservableCollection<Meal> mealPickerOptions;

        private async Task LoadMealPickerOptions()
        {
            try
            {
                IsBusy = true;
                MealPickerOptions = await _mealService.GetAllMeals();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Unable to fetch meals", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        void AddMealToScheduleItem()
        {
            if (SelectedMeal == null) return;
            Meals.Add(SelectedMeal);
        }

        [RelayCommand]
        void RemoveMealFromScheduleItem(Meal meal)
        {
            if (SelectedMeal == null) return;
            Meals.Remove(meal);
        }

        [RelayCommand]
        async Task UpdateMealSessionAsync()
        {
            if (IsBusy) return;
            
            try
            {
                IsBusy = true;
                MealSession.MealIds = Meals.Select(meal => meal.MealId.ToString()).ToList();
                MealSession.From = MealStartTime;
                MealSession.To = MealStartTime.AddMinutes(MealDuration);
                await _schedulerService.UpdateScheduleItem(MealSession.ScheduleItemId.ToString(), MealSession);
                await Shell.Current.GoToAsync($"{nameof(SchedulerPage)}?Reload={true}");
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

        public void InitSelectedDateTime()
        {
            SelectedDate = MealSession.From;
            SelectedTime = MealSession.From.TimeOfDay;
            MealDuration = (MealSession.To - MealSession.From).Minutes;
        }
    }
}
