using Diabot.ViewModels.Scheduler;
using Syncfusion.Maui.Scheduler;

namespace Diabot.Views.Scheduler;

public partial class SchedulerPage : ContentPage
{
    private readonly SchedulerViewModel _vm;

	public SchedulerPage(SchedulerViewModel vm)
	{
		InitializeComponent();
		BindingContext = _vm = vm;
        Scheduler.Tapped += OnSchedulerTapped;
	}

    private void OnSchedulerTapped(object sender, SchedulerTappedEventArgs e)
    {
        var appointments = e.Appointments;
        var selectedDate = e.Date;

        if (appointments is null)
        {
            _vm.GoToScheduleMealSessionPageCommand.Execute(selectedDate);
            return;
        }

        if (appointments.Count > 0)
        {
            var mealSession = appointments[0];
            _vm.GoToMealSessionDetailsPageCommand.Execute(mealSession);
        }
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (_vm != null && _vm.FetchAllMealsCommand.CanExecute(null) && _vm.Reload)
        {
            _vm.FetchAllMealsCommand.ExecuteAsync(null);
            _vm.Reload = false;
        }
        base.OnNavigatedTo(args);
    }
}