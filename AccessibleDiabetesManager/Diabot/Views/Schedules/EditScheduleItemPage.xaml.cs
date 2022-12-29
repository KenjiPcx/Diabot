using Diabot.ViewModels.Scheduler;

namespace Diabot.Views.Scheduler;

public partial class EditScheduleItemPage : ContentPage
{
	public EditScheduleItemPage(EditScheduleItemViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}