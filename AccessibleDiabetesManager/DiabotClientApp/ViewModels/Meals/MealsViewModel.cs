using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diabot.Models;
using Diabot.Services.Interfaces;
using Diabot.Views.Meals;
using System.Collections.ObjectModel;

namespace Diabot.ViewModels.Meals
{
    [QueryProperty(nameof(Reload), "Reload")]
    public partial class MealsViewModel : BaseViewModel
    {
        private readonly IMealService _mealService;
        private readonly IConnectivity _connectivity;
        public MealsViewModel(IMealService mealService, IConnectivity connectivity)
        {
            Title = "All Saved Meals";
            _mealService = mealService;
            _connectivity = connectivity;
            Task.Run(GetAllMealsAsync);
        }

        public bool Reload { get; set; }

        [ObservableProperty]
        ObservableCollection<Meal> meals;

        [ObservableProperty]
        bool isRefreshing;

        [RelayCommand]
        async Task GetAllMealsAsync()
        {
            if (IsBusy) return;

            try
            {
                if (_connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("No connectivity!", $"Please check internet and try again.", "Ok");
                    return;
                }

                IsBusy = true;
                var meals = await _mealService.GetAllMeals();

                Meals = new ObservableCollection<Meal>(meals);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        async Task GoToDetailsAsync(Meal meal)
        {
            if (IsBusy) return;
            if (meal is null)  return;

            await Shell.Current.GoToAsync($"{nameof(MealDetailsPage)}", true, new Dictionary<string, object>
            {
                { "Meal", meal }
            });
        }

        [RelayCommand]
        async Task GoToAddFormAsync()
        {
            if (IsBusy) return;
            await Shell.Current.GoToAsync($"{nameof(AddMealPage)}", true);
        }
    }
}
