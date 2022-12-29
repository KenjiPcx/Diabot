using Diabot.ViewModels.Scheduler;

namespace Diabot.Views.Scheduler;

public partial class EditScheduleItemPage : ContentPage
{
    private readonly EditScheduleItemViewModel _vm;

	public EditScheduleItemPage(EditScheduleItemViewModel vm)
	{
		InitializeComponent();
		BindingContext = _vm = vm;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (_vm != null && _vm.FetchAllMealsCommand.CanExecute(null))
        {
            _vm.FetchAllMealsCommand.ExecuteAsync(null);
        }
        _vm.InitSelectedDateTime();
        base.OnNavigatedTo(args);
    }
}