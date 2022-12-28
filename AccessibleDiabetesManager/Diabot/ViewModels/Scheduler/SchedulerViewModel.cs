using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diabot.Models;
using Diabot.Services.Interfaces;
using Syncfusion.Maui.Scheduler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diabot.ViewModels.Scheduler
{
    public partial class SchedulerViewModel : BaseViewModel
    {
        private readonly ISchedulerService _schedulerService;

        public SchedulerViewModel(ISchedulerService schedulerService)
        {
            _schedulerService = schedulerService;
            Title = "Meal Schedule";
            Meals = new ObservableCollection<ScheduleItem> {
                new ScheduleItem
                {
                    Notes = "Can;t wait",
                    MealName = "Fried Rice",
                    ScheduleItemId = Guid.NewGuid(),
                    Background = new SolidColorBrush(Color.FromArgb("#FF8B1FA9")),
                    From = DateTime.Now,
                    To = DateTime.Now.AddMinutes(60),
                } 
            };
        }

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
        async Task GoToAddScheduleItemPage(ScheduleItem item)
        {
            if (IsBusy) return;

            await Shell.Current.GoToAsync($"{nameof(GoToAddScheduleItemPage)}", true, new Dictionary<string, object>
            {
                { "ScheduleItem", item }
            });
        }
    }
}
