using Diabot.ViewModels;

namespace Diabot.Views;

public partial class AddMealPage : ContentPage
{
	public AddMealPage(AddMealViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}