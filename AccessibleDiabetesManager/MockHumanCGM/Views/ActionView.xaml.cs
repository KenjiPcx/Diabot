using MockHumanCGM.ViewModels;
using System.Diagnostics;
using System.Linq;
using CommunityToolkit.Mvvm.Messaging;

namespace MockHumanCGM.Views;

public partial class ActionsView : ContentPage
{
    public ActionsView()
    {
		InitializeComponent();
        BindingContext = new ActionsViewModel();
    }

    public void OnCarbsEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        newCarbs.Text = string.Concat(e.NewTextValue.Where(c => char.IsDigit(c)).ToArray());
    }
}