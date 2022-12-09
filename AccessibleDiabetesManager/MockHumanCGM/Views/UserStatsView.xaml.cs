using MockHumanCGM.ViewModels;

namespace MockHumanCGM.Views;

public partial class UserStatsView : ContentPage
{
    public UserStatsView(UserStatsViewModel userStatsVm)
	{
		InitializeComponent();
        BindingContext = userStatsVm;
    }
}

