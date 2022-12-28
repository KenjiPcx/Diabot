using Diabot.Views;

namespace Diabot
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MealsPage), typeof(MealsPage));
            Routing.RegisterRoute(nameof(MealDetailsPage), typeof(MealDetailsPage));
            Routing.RegisterRoute(nameof(AddMealPage), typeof(AddMealPage));
            Routing.RegisterRoute(nameof(EditMealPage), typeof(EditMealPage));
        }
    }
}