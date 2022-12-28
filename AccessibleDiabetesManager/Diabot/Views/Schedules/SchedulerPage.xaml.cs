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
        var selectedDate = e.Date;
        var schedulerElement = e.Element;

        if (appointments.Count > 0)
        {
            _vm.
        }
    }
}