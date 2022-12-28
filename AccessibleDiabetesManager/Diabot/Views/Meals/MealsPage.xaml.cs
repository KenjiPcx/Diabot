using Diabot.ViewModels.Meals;

namespace Diabot.Views.Meals;

public partial class MealsPage : ContentPage
{
	private readonly MealsViewModel _vm;
	public MealsPage(MealsViewModel vm)
	{
		InitializeComponent();
		BindingContext = _vm = vm;
	}
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
		if (_vm != null && _vm.GetAllMealsCommand.CanExecute(null) && _vm.Reload)
		{
			_vm.GetAllMealsCommand.ExecuteAsync(null);
			_vm.Reload = false;
		}
        base.OnNavigatedTo(args);
    }
}