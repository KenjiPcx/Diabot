using Diabot.ViewModels.Meals;

namespace Diabot.Views.Meals;

public partial class AddMealPage : ContentPage
{
	public AddMealPage(AddMealViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}