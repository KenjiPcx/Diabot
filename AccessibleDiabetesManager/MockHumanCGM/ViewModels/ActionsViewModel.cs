using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MockHumanCGM.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockHumanCGM.ViewModels
{
    public partial class ActionsViewModel : ObservableObject
    {
        [ObservableProperty]
        string newCarbs;

        [ObservableProperty]
        string newInsulin;

        [RelayCommand]
        void EatFood()
        {
            if (NewCarbs == string.Empty) NewCarbs = "0";
            WeakReferenceMessenger.Default.Send(new UpdateCarbsMessage(int.Parse(NewCarbs)));
            NewCarbs = string.Empty;
        }

        [RelayCommand]
        void InjectInsulin()
        {
            WeakReferenceMessenger.Default.Send(new UpdateInsulinMessage(1));
            NewInsulin = string.Empty;
        }
    }
}
