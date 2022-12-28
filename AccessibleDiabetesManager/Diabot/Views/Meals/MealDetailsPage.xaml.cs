using Diabot.ViewModels;

namespace Diabot.Views;

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