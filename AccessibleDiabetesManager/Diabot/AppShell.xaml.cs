using Diabot.Views.Home;
using Diabot.Views.Meals;
using Diabot.Views.Scheduler;

namespace Diabot
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Meals
            Routing.RegisterRoute(nameof(MealsPage), typeof(MealsPage));
            Routing.RegisterRoute(nameof(MealDetailsPage), typeof(MealDetailsPage));
            Routing.RegisterRoute(nameof(AddMealPage), typeof(AddMealPage));
            Routing.RegisterRoute(nameof(EditMealPage), typeof(EditMealPage));

            // Scheduler
            Routing.RegisterRoute(nameof(SchedulerPage), typeof(SchedulerPage));
            Routing.RegisterRoute(nameof(ScheduleItemDetailsPage), typeof(ScheduleItemDetailsPage));
            Routing.RegisterRoute(nameof(AddScheduleItemPage), typeof(AddScheduleItemPage));
            Routing.RegisterRoute(nameof(EditScheduleItemPage), typeof(EditScheduleItemPage));

            // Home
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        }
    }
}