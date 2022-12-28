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
	}

    private void OnSchedulerTapped(object sender, SchedulerTappedEventArgs e)
    {
        var appointments = e.Appointments;

        if (appointments.Count > 0)
        {
            var mealSession = appointments[0];
            _vm.GoToMealSessionDetailsPageCommand.Execute(mealSession);
        } 
        else
        {
            _vm.GoToScheduleMealSessionPageCommand.Execute(null);
        }
    }
}