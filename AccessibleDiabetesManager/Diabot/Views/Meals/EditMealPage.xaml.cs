using Diabot.ViewModels.Meals;

namespace Diabot.Views.Meals;

public partial class EditMealPage : ContentPage
{
	public EditMealPage(EditMealViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}