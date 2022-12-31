using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diabot.Models;
using Diabot.Services.Interfaces;
using Diabot.Views.Scheduler;
using System.Collections.ObjectModel;

namespace Diabot.ViewModels.Scheduler
{
    [QueryProperty(nameof(Reload), "Reload")]
    public partial class SchedulerViewModel : BaseViewModel
    {
        private readonly ISchedulerService _schedulerService;

        public SchedulerViewModel(ISchedulerService schedulerService)
        {
            _schedulerService = schedulerService;
            Title = "Meal Schedule";
            Task.Run(FetchAllMeals);
        }

        public bool Reload { get; set; }

        [ObservableProperty]
        ObservableCollection<ScheduleItem> meals;

        [RelayCommand]
        async Task FetchAllMeals()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                Meals = await _schedulerService.GetAllScheduleItems();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error fetching scheduled meals", ex.Message, "Ok");
            }
            finally
            { 
                IsBusy = false; 
            }
        }

        [RelayCommand]
        async Task GoToScheduleMealSessionPage(DateTime selectedDateTime)
        {
            if (IsBusy) return;

            await Shell.Current.GoToAsync($"{nameof(AddScheduleItemPage)}", true, new Dictionary<string, object>
            {
                { "SelectedDate", selectedDateTime.Date },
                { "SelectedTime", selectedDateTime.TimeOfDay }
            });
        }

        [RelayCommand]
        async Task GoToMealSessionDetailsPage(ScheduleItem mealSession)
        {
            if (IsBusy) return;

            await Shell.Current.GoToAsync($"{nameof(ScheduleItemDetailsPage)}", true, new Dictionary<string, object>
            {
                { "MealSession", mealSession }
            });
        }
    }
}
