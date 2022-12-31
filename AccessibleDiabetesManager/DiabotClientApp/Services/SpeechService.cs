using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diabot.Services
{
    public class SpeechService
    {
        private readonly string _speechKey = Environment.GetEnvironmentVariable("SPEECH_KEY");
        private readonly string _speechRegion = Environment.GetEnvironmentVariable("SPEECH_REGION");
        private readonly SpeechConfig _speechConfig;

        public SpeechService() 
        {
            _speechConfig = SpeechConfig.FromSubscription(_speechKey, _speechRegion);
            _speechConfig.SpeechRecognitionLanguage = "en-us";
        }

        public async Task<string> ListenToUserVoiceInput()
        {
            using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            using var speechRecognizer = new SpeechRecognizer(_speechConfig, audioConfig);
            var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();

            if (speechRecognitionResult.Reason == ResultReason.RecognizedSpeech) 
                return speechRecognitionResult.Text;
            return "";
        }
    }
}
