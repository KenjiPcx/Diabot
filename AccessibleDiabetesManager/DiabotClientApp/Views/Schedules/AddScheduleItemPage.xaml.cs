using Diabot.ViewModels.Scheduler;

namespace Diabot.Views.Scheduler;

public partial class AddScheduleItemPage : ContentPage
{
	public AddScheduleItemPage(AddScheduleItemViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}