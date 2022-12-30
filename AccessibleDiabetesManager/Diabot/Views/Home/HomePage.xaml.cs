using Diabot.ViewModels.Home;

namespace Diabot.Views.Home;

public partial class HomePage : ContentPage
{
	public HomePage(HomeViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}