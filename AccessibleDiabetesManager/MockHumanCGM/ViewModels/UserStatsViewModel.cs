using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class UserStatsViewModel : ObservableObject, IRecipient<UpdateCarbsMessage>, IRecipient<UpdateInsulinMessage>
    {
        public UserStatsViewModel() 
        {
            carbLossRate = activeCarbohydrates / foodDigestTime;
            carbGainRate = carbLossRate * carbToGlucoseRatio;
            glucoseLossRate = insulinRatio / insulinActiveTime;
            insulinLossRate = 1.0 / insulinActiveTime;
            Task.Run(() => StartBackgroundService());

            WeakReferenceMessenger.Default.Register<UpdateCarbsMessage>(this);
            WeakReferenceMessenger.Default.Register<UpdateInsulinMessage>(this);
        }
        
        [ObservableProperty]
        HumanStatus status = HumanStatus.Normal;
        [ObservableProperty]
        double glucoseLevels = 125;
        [ObservableProperty] 
        double activeCarbohydrates = 0;
        [ObservableProperty]
        double activeInsulinLevels = 1.0;


        private double carbLossRate;
        private double carbGainRate;
        private const int carbToGlucoseRatio = 4;
        private const int foodDigestTime = 90;

        private double glucoseLossRate;
        private double insulinLossRate;
        private const double insulinRatio = 40;
        private const int insulinActiveTime = 120;

        public void Receive(UpdateCarbsMessage message)
        {
            Task.Delay(15000).ContinueWith(t =>
            {
                ActiveCarbohydrates += message.Value;
                carbLossRate = ActiveCarbohydrates / foodDigestTime;
                carbGainRate = carbLossRate * carbToGlucoseRatio;
            });
        }
        
        public void Receive(UpdateInsulinMessage message)
        {
            Task.Delay(15000).ContinueWith(t =>
            {
                ActiveInsulinLevels += message.Value;
                glucoseLossRate = insulinRatio / insulinActiveTime;
            });
        }

        public async Task StartBackgroundService()
        {
            PeriodicTimer timer = new(TimeSpan.FromSeconds(1));
            while (await timer.WaitForNextTickAsync()) 
            {
                if (GlucoseLevels < 70) Status = HumanStatus.LowOnGlucose;
                if (GlucoseLevels > 180) Status = HumanStatus.TooMuchGlucose;
                if (ActiveInsulinLevels == 0) Status = HumanStatus.LowOnInsulin;

                if (ActiveCarbohydrates > 0)
                {
                    ActiveCarbohydrates -= carbLossRate;
                    GlucoseLevels += carbGainRate;
                }

                if (ActiveInsulinLevels > 0)
                {
                    GlucoseLevels -= glucoseLossRate;
                    ActiveInsulinLevels -= insulinLossRate;
                }

               if (ActiveInsulinLevels < 0) ActiveInsulinLevels= 0;
               if (ActiveCarbohydrates < 0) ActiveCarbohydrates= 0;
            }
        }
    }

    public enum HumanStatus
    {
        Normal,
        LowOnGlucose,
        TooMuchGlucose,
        LowOnInsulin,
    }
}
