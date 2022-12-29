using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diabot.Models;
using Diabot.Services.Interfaces;
using Diabot.Views.Scheduler;
using System.Collections.ObjectModel;

namespace Diabot.ViewModels.Scheduler
{
    [QueryProperty(nameof(SelectedDate), "SelectedDate")]
    [QueryProperty(nameof(SelectedTime), "SelectedTime")]
    public partial class AddScheduleItemViewModel : BaseViewModel
    {
        private readonly ISchedulerService _schedulerService;
        private readonly IMealService _mealService;
        
        public AddScheduleItemViewModel(ISchedulerService schedulerService, IMealService mealService) 
        {
            _schedulerService = schedulerService;
            _mealService = mealService;
            Task.Run(LoadMealPickerOptionsAsync);
            Title = "Schedule a meal session";
        }

        [ObservableProperty]
        string mealSessionName;

        [ObservableProperty]
        string notes;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MealStartTime))]
        DateTime selectedDate;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MealStartTime))]
        TimeSpan selectedTime;

        public DateTime MealStartTime => SelectedDate.Add(SelectedTime);

        [ObservableProperty]
        int mealDuration;

        [ObservableProperty]
        ObservableCollection<Meal> mealPickerOptions;

        [ObservableProperty]
        ObservableCollection<Meal> meals = new();
        
        [ObservableProperty]
        Meal selectedMeal;
        
        private async Task LoadMealPickerOptionsAsync()
        {
            if (IsBusy) return;

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
            Meals.Add(selectedMeal);
        }

        [RelayCommand]
        void RemoveMealFromScheduleItem()
        {
            Meals.Remove(selectedMeal);
        }

        [RelayCommand]
        async Task ScheduleMealSessionAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                var item = new ScheduleItem
                {
                    MealName = MealSessionName,
                    From = MealStartTime,
                    To = MealStartTime.AddMinutes(MealDuration),
                    Notes = Notes,
                    MealIds = Meals.Select(meal => meal.MealId.ToString()).ToList(),
                };
                await _schedulerService.AddScheduleItem(item);
                await Shell.Current.GoToAsync($"{nameof(SchedulerPage)}?Reload={true}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
            finally
            { 
                IsBusy = false; 
            }
        }
    }
}
