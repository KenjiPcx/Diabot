using Diabot.ViewModels.Meals;

namespace Diabot.Views.Meals;

public partial class MealDetailsPage : ContentPage
{
	private readonly MealDetailsViewModel _vm;
	public MealDetailsPage(MealDetailsViewModel vm)
	{
		InitializeComponent();
		BindingContext = _vm = vm;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (_vm != null && _vm.AggregateNutritionCommand.CanExecute(null))
        {
            _vm.AggregateNutritionCommand.Execute(null);
        }
        base.OnNavigatedTo(args);
    }
}