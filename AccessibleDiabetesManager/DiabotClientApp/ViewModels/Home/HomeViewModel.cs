using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diabot.Models;
using Diabot.Services.Interfaces;
using Microsoft.CognitiveServices.Speech.Audio;
using System.Collections.ObjectModel;
using Microsoft.CognitiveServices.Speech.Dialog;
using Plugin.Maui.Audio;
using System.Text;
using Microsoft.CognitiveServices.Speech;
using Newtonsoft.Json;

namespace Diabot.ViewModels.Home
{
    public partial class HomeViewModel : BaseViewModel
    {
        private readonly ISchedulerService _schedulerService;
        private readonly IAudioManager _audioManager;
        private readonly string _speechKey = "<YOUR SPEECH SERVICE KEY";
        private readonly string _speechRegion = "westus";
        private DialogServiceConnector _connector;

        public HomeViewModel(ISchedulerService schedulerService, IAudioManager audioManager) 
        {
            _schedulerService = schedulerService;
            Title = "Home";

            Task.Run(InitHomePageAsync);
            _audioManager = audioManager;
        }

        [ObservableProperty]
        ObservableCollection<ScheduleItem> allMeals;

        [ObservableProperty]
        ObservableCollection<TimeSeriesChartPoint> glucoseLevels;

        [ObservableProperty]
        ObservableCollection<TimeSeriesChartPoint> loggedMeals;

        [ObservableProperty]
        DateTime maximumDateForChart;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NextMealDoesNotExists))]
        bool nextMealExists;

        public bool NextMealDoesNotExists => !nextMealExists;

        [ObservableProperty]
        ScheduleItem nextMeal;

        [ObservableProperty]
        double currentGlucoseLevel;

        public List<Brush> GlucoseLevelsBrushes => new List<Brush> { new SolidColorBrush(Color.FromArgb("#512BD4")) };
        public List<Brush> LoggedMealsBrushes => new List<Brush> { new SolidColorBrush(Color.FromArgb("#2B0B98")) };

        private async Task InitHomePageAsync()
        {
            PopulateGlucoseLevelData();
            await FetchLoggedMealsDataAsync();
            PopulateLoggedMealsData();
            PopulateNextMeal();
        }

        [RelayCommand]
        async Task FetchLoggedMealsDataAsync()
        {
            try
            {
                IsBusy = true;
                var lastDate = GlucoseLevels.Last();
                AllMeals = await _schedulerService.GetAllScheduleItemsBeforeDatetime(lastDate.Timestamp.AddHours(12));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error fetching data", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void PopulateLoggedMealsData()
        {
            LoggedMeals = new ObservableCollection<TimeSeriesChartPoint>(
                    AllMeals.Select(meal => new TimeSeriesChartPoint
                    {
                        Timestamp = meal.From,
                        Value = GetGlucoseLevel(meal.From)
                    }).ToList()); ;
        }

        private double GetGlucoseLevel(DateTime timestamp)
        {
            var reading = GlucoseLevels.FirstOrDefault(r => r.Timestamp == timestamp);
            if (reading is null) return 100;
            return reading.Value;
        }

        private void PopulateNextMeal()
        {
            var today = DateTime.Today.Date;
            var nextMeal = AllMeals.FirstOrDefault(meal => meal.From.Date == today && meal.From >= DateTime.Now);
            NextMeal = nextMeal;
            NextMealExists = nextMeal != null;
        }   
        
        private void PopulateGlucoseLevelData()
        {
            double currentGlucoseLevel = 70;
            var currentTime = DateTime.Now.Date.AddDays(-1).AddHours(7);
            var pattern = new List<double> { -0.25, 1.5, -0.5, -0.25, -0.25, 1.75, -0.5, -0.25, 0.5, -0.25, -0.25, -0.25, 1.75, -0.5, -0.25, -0.25, -0.125, -0.125, -0.125, -0.125, -0.125, -0.125 };
            var random = new Random();

            GlucoseLevels = new ObservableCollection<TimeSeriesChartPoint>
            {
                new TimeSeriesChartPoint
                {
                    Timestamp = currentTime,
                    Value = currentGlucoseLevel
                }
            };
            for (var i = 0; i < pattern.Count; i++)
            {
                var trend = pattern[i];

                for (var j = 0; j < 12; j++)
                {
                    currentTime = currentTime.AddMinutes(5);
                    currentGlucoseLevel += trend;
                    GlucoseLevels.Add(new TimeSeriesChartPoint
                    {
                        Timestamp = currentTime,
                        Value = currentGlucoseLevel + random.Next(-1, 1)
                    });
                }
            }
            var lastReading = GlucoseLevels.Last();
            MaximumDateForChart = lastReading.Timestamp.AddHours(3);
            CurrentGlucoseLevel = lastReading.Value;
        }

        // Integration with the bot doesn't work :(

        //[ObservableProperty]
        //string botStatus;

        //[ObservableProperty]
        //string saidMsg;

        //private enum NotifyType
        //{
        //    StatusMessage,
        //    ErrorMessage
        //};

        //private void NotifyUser(
        //    string strMessage, NotifyType type = NotifyType.StatusMessage)
        //{
        //    BotStatus = strMessage;
        //}

        //private async Task InitializeDialogServiceConnector()
        //{
        //    DialogServiceConfig config = null;
        //    config = BotFrameworkConfig.FromSubscription(_speechKey, _speechRegion);
        //    config.Language = "en-us";
        //    _connector = new DialogServiceConnector(config, AudioConfig.FromDefaultMicrophoneInput());

        //    _connector.ActivityReceived += async (sender, activityReceivedEventArgs) =>
        //    {
        //        NotifyUser(
        //            $"Activity received, hasAudio={activityReceivedEventArgs.HasAudio} activity={activityReceivedEventArgs.Activity}");
        //        dynamic dyn = JsonConvert.DeserializeObject(activityReceivedEventArgs.Activity);
        //        string speak = (string)dyn["speak"];
        //        await TextToSpeech.Default.SpeakAsync(speak);
        //        //if (activityReceivedEventArgs.HasAudio)
        //        //{
        //        //    SynchronouslyPlayActivityAudio(activityReceivedEventArgs.Audio);
        //        //}
        //    };

        //    // Canceled will be signaled when a turn is aborted or experiences an error condition
        //    _connector.Canceled += (sender, canceledEventArgs) =>
        //    {
        //        NotifyUser($"Canceled, reason={canceledEventArgs.Reason}");
        //        if (canceledEventArgs.Reason == CancellationReason.Error)
        //        {
        //            NotifyUser(
        //                $"Error: code={canceledEventArgs.ErrorCode}, details={canceledEventArgs.ErrorDetails}");
        //        }
        //    };

        //    // Recognizing (not 'Recognized') will provide the intermediate recognized text 
        //    // while an audio stream is being processed
        //    _connector.Recognizing += (sender, recognitionEventArgs) =>
        //    {
        //        NotifyUser($"Recognizing! in-progress text={recognitionEventArgs.Result.Text}");
        //    };

        //    // Recognized (not 'Recognizing') will provide the final recognized text 
        //    // once audio capture is completed
        //    _connector.Recognized += (sender, recognitionEventArgs) =>
        //    {
        //        NotifyUser($"Final speech-to-text result: '{recognitionEventArgs.Result.Text}'");
        //    };

        //    // SessionStarted will notify when audio begins flowing to the service for a turn
        //    _connector.SessionStarted += (sender, sessionEventArgs) =>
        //    {
        //        NotifyUser($"Now Listening! Session started, id={sessionEventArgs.SessionId}");
        //    };

        //    // SessionStopped will notify when a turn is complete and 
        //    // it's safe to begin listening again
        //    _connector.SessionStopped += (sender, sessionEventArgs) =>
        //    {
        //        NotifyUser($"Listening complete. Session ended, id={sessionEventArgs.SessionId}");
        //    };

        //    await _connector.ConnectAsync();
        //}

        //private void SynchronouslyPlayActivityAudio(
        //    PullAudioOutputStream activityAudio)
        //{
        //    var playbackStreamWithHeader = new MemoryStream();
        //    playbackStreamWithHeader.Write(Encoding.ASCII.GetBytes("RIFF"), 0, 4); // ChunkID
        //    playbackStreamWithHeader.Write(BitConverter.GetBytes(UInt32.MaxValue), 0, 4); // ChunkSize: max
        //    playbackStreamWithHeader.Write(Encoding.ASCII.GetBytes("WAVE"), 0, 4); // Format
        //    playbackStreamWithHeader.Write(Encoding.ASCII.GetBytes("fmt "), 0, 4); // Subchunk1ID
        //    playbackStreamWithHeader.Write(BitConverter.GetBytes(16), 0, 4); // Subchunk1Size: PCM
        //    playbackStreamWithHeader.Write(BitConverter.GetBytes(1), 0, 2); // AudioFormat: PCM
        //    playbackStreamWithHeader.Write(BitConverter.GetBytes(1), 0, 2); // NumChannels: mono
        //    playbackStreamWithHeader.Write(BitConverter.GetBytes(16000), 0, 4); // SampleRate: 16kHz
        //    playbackStreamWithHeader.Write(BitConverter.GetBytes(32000), 0, 4); // ByteRate
        //    playbackStreamWithHeader.Write(BitConverter.GetBytes(2), 0, 2); // BlockAlign
        //    playbackStreamWithHeader.Write(BitConverter.GetBytes(16), 0, 2); // BitsPerSample: 16-bit
        //    playbackStreamWithHeader.Write(Encoding.ASCII.GetBytes("data"), 0, 4); // Subchunk2ID
        //    playbackStreamWithHeader.Write(BitConverter.GetBytes(UInt32.MaxValue), 0, 4); // Subchunk2Size

        //    byte[] pullBuffer = new byte[2056];

        //    uint lastRead = 0;
        //    do
        //    {
        //        lastRead = activityAudio.Read(pullBuffer);
        //        playbackStreamWithHeader.Write(pullBuffer, 0, (int)lastRead);
        //    }
        //    while (lastRead == pullBuffer.Length);

        //    using var player = _audioManager.CreatePlayer(playbackStreamWithHeader);
        //    player.Play();
        //}

        //[RelayCommand]
        //async Task ListenToUserVoiceInput()
        //{
        //    if (_connector == null)
        //    {
        //        await InitializeDialogServiceConnector();
        //    }

        //    try
        //    {
        //        var audio = AudioConfig.FromDefaultMicrophoneInput();
                
        //        // Start sending audio to your speech-enabled bot
        //        var listenTask =  _connector.ListenOnceAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        NotifyUser($"Exception: {ex}", NotifyType.ErrorMessage);
        //    }
        //}
    }
}
