using System;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using CognitiveServicesSpeechTest.Consts;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CognitiveServicesSpeechTest.Operations
{
	public class SpeechToTextContinuous
    {
        SpeechConfig _config;
        TaskCompletionSource<int> _stopRecognition;

        public SpeechToTextContinuous()
        {
            _config = SpeechConfig.FromSubscription(AppConsts.CognitiveServicesApiKey, AppConsts.CognitiveServicesRegion);
        }
        
        public async Task<bool> StartContinuousRecognizeAsync()
        {
            _stopRecognition = new TaskCompletionSource<int>();
            using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            using var recognizer = new SpeechRecognizer(_config, audioConfig);
            recognizer.Recognizing += (s, e) =>
            {
                Console.WriteLine($"RECOGNIZING: Text={e.Result.Text}");
            };

            recognizer.Recognized += (s, e) =>
            {
                if (e.Result.Reason == ResultReason.RecognizedSpeech)
                {
                    Console.WriteLine($"RECOGNIZED: Text={e.Result.Text}");
                }
                else if (e.Result.Reason == ResultReason.NoMatch)
                {
                    Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                }
            };

            recognizer.Canceled += (s, e) =>
            {
                Console.WriteLine($"CANCELED: Reason={e.Reason}");

                if (e.Reason == CancellationReason.Error)
                {
                    Console.WriteLine($"CANCELED: ErrorCode={e.ErrorCode}");
                    Console.WriteLine($"CANCELED: ErrorDetails={e.ErrorDetails}");
                    Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
                }

                _stopRecognition.TrySetResult(0);
            };

            recognizer.SessionStopped += (s, e) =>
            {
                Console.WriteLine("\n    Session stopped event.");
                _stopRecognition.TrySetResult(0);
            };

            recognizer.SessionStarted += (s, e) =>
            {
                Console.WriteLine("\n    Session start event.");                
            };

            await recognizer.StartContinuousRecognitionAsync();            

            // Waits for completion. Use Task.WaitAny to keep the task rooted.
            Task.WaitAny(new[] { _stopRecognition.Task });

            // Make the following call at some point to stop recognition:
            await recognizer.StopContinuousRecognitionAsync();
            //
            return true;
        }

        public void StoptContinuousRecognizeAsync()
        {
            Console.WriteLine("StoptContinuousRecognizeAsync");
            _stopRecognition.TrySetResult(0);           
        }
    }
}

