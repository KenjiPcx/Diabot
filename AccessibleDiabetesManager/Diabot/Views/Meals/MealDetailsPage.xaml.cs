using Diabot.ViewModels.Meals;

namespace Diabot.Views.Meals;

public partial class MealDetailsPage : ContentPage
{
	public MealDetailsPage(MealDetailsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}