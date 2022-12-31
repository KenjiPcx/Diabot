using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diabot.Models;
using Diabot.Services.Interfaces;
using System.Collections.ObjectModel;

namespace Diabot.ViewModels.Home
{
    public partial class HomeViewModel : BaseViewModel
    {
        private readonly ISchedulerService _schedulerService;
        public HomeViewModel(ISchedulerService schedulerService) 
        {
            _schedulerService = schedulerService;
            Title = "Home";

            Task.Run(InitHomePageAsync);
        }

        [ObservableProperty]
        ObservableCollection<ScheduleItem> allMeals;

        [ObservableProperty]
        ObservableCollection<TimeSeriesChartPoint> glucoseLevels;

        [ObservableProperty]
        ObservableCollection<TimeSeriesChartPoint> loggedMeals;

        [ObservableProperty]
        DateTime maximumDateForChart;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NextMealDoesNotExists))]
        bool nextMealExists;

        public bool NextMealDoesNotExists => !nextMealExists;

        [ObservableProperty]
        ScheduleItem nextMeal;

        [ObservableProperty]
        double currentGlucoseLevel;

        public List<Brush> GlucoseLevelsBrushes => new List<Brush> { new SolidColorBrush(Color.FromArgb("#512BD4")) };
        public List<Brush> LoggedMealsBrushes => new List<Brush> { new SolidColorBrush(Color.FromArgb("#2B0B98")) };

        private async Task InitHomePageAsync()
        {
            PopulateGlucoseLevelData();
            await FetchLoggedMealsDataAsync();
            PopulateLoggedMealsData();
            PopulateNextMeal();
        }

        [RelayCommand]
        async Task FetchLoggedMealsDataAsync()
        {
            try
            {
                IsBusy = true;
                var lastDate = GlucoseLevels.Last();
                AllMeals = await _schedulerService.GetAllScheduleItemsBeforeDatetime(lastDate.Timestamp.AddHours(12));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error fetching data", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void PopulateLoggedMealsData()
        {
            LoggedMeals = new ObservableCollection<TimeSeriesChartPoint>(
                    AllMeals.Select(meal => new TimeSeriesChartPoint
                    {
                        Timestamp = meal.From,
                        Value = GetGlucoseLevel(meal.From)
                    }).ToList()); ;
        }

        private double GetGlucoseLevel(DateTime timestamp)
        {
            var reading = GlucoseLevels.FirstOrDefault(r => r.Timestamp == timestamp);
            if (reading is null) return 100;
            return reading.Value;
        }

        private void PopulateNextMeal()
        {
            var today = DateTime.Today.Date;
            var nextMeal = AllMeals.FirstOrDefault(meal => meal.From.Date == today && meal.From >= DateTime.Now);
            NextMeal = nextMeal;
            NextMealExists = nextMeal != null;
        }   
        
        private void PopulateGlucoseLevelData()
        {
            double currentGlucoseLevel = 70;
            var currentTime = DateTime.Now.Date.AddDays(-1).AddHours(7);
            var pattern = new List<double> { -0.25, 1.5, -0.5, -0.25, -0.25, 1.75, -0.5, -0.25, 0.5, -0.25, -0.25, -0.25, 1.75, -0.5, -0.25, -0.25, -0.125, -0.125, -0.125, -0.125, -0.125, -0.125 };
            var random = new Random();

            GlucoseLevels = new ObservableCollection<TimeSeriesChartPoint>
            {
                new TimeSeriesChartPoint
                {
                    Timestamp = currentTime,
                    Value = currentGlucoseLevel
                }
            };
            for (var i = 0; i < pattern.Count; i++)
            {
                var trend = pattern[i];

                for (var j = 0; j < 12; j++)
                {
                    currentTime = currentTime.AddMinutes(5);
                    currentGlucoseLevel += trend;
                    GlucoseLevels.Add(new TimeSeriesChartPoint
                    {
                        Timestamp = currentTime,
                        Value = currentGlucoseLevel + random.Next(-1, 1)
                    });
                }
            }
            var lastReading = GlucoseLevels.Last();
            MaximumDateForChart = lastReading.Timestamp.AddHours(3);
            CurrentGlucoseLevel = lastReading.Value;
        }
    }
}
