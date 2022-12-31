using Diabot.ViewModels.Scheduler;

namespace Diabot.Views.Scheduler;

public partial class ScheduleItemDetailsPage : ContentPage
{
    private readonly ScheduleItemDetailsViewModel _vm;
	public ScheduleItemDetailsPage(ScheduleItemDetailsViewModel vm)
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
        base.OnNavigatedTo(args);
    }
}