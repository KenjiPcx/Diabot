using MockHumanCGM.ViewModels;
using System.Diagnostics;

namespace MockHumanCGM.Views;

public partial class UserStatsView : ContentPage
{
	public UserStatsView(UserStatsViewModel userStatsVm)
	{
		InitializeComponent();
		BindingContext = userStatsVm;
	}
}

