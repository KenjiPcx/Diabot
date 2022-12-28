using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diabot.Models;
using Diabot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diabot.ViewModels.Scheduler
{
    public partial class AddScheduleItemViewModel : BaseViewModel
    {
        private readonly ISchedulerService _schedulerService;
        private readonly IMealService _mealService;
        
        public AddScheduleItemViewModel(ISchedulerService schedulerService, IMealService mealService) 
        {
            _schedulerService = schedulerService;
            _mealService = mealService;
            Title = "Schedule a meal session";
            Task.Run(LoadMealPickerOptions);
        }

        [ObservableProperty]
        string mealName;

        [ObservableProperty]
        DateTime from;

        [ObservableProperty]
        DateTime to;

        [ObservableProperty]
        ObservableCollection<Meal> meals;

        [ObservableProperty]
        Meal selectedMeal;

        [ObservableProperty]
        string notes;

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
        async Task AddScheduleItemAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                var item = new ScheduleItem
                {
                    MealName = MealName,
                    From = From,
                    To = To,
                    Notes = Notes,
                    MealIds = Meals.Select(meal => meal.MealId.ToString()).ToList(),
                };
                await _schedulerService.AddScheduleItem(item);
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
