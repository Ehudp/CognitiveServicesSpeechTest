using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CognitiveServicesSpeechTest.Consts;
using CognitiveServicesSpeechTest.Services;
using Microsoft.CognitiveServices.Speech;
using Xamarin.Forms;

namespace CognitiveServicesSpeechTest.Operations
{
    public class SpeechToTextWithKeywordContinuous
    {
        private string kwsModelFile = "kws.table";
        SpeechConfig _config;

        public SpeechToTextWithKeywordContinuous()
        {
            _config = SpeechConfig.FromSubscription(AppConsts.CognitiveServicesApiKey, AppConsts.CognitiveServicesRegion);
        }

        public async Task StartContinuousRecognizeAsync()
        {
            try
            {
                // Creates an instance of a speech config with specified subscription key and service region.
                // Replace with your own subscription key and service region (e.g., "westus").
                //var config = SpeechConfig.FromSubscription(CognitiveServicesApiKey, CognitiveServicesRegion);

                var kwsModelDir = DependencyService.Get<IAssetService>().GetAssetPath(kwsModelFile);
                var model = KeywordRecognitionModel.FromFile(kwsModelDir);

                // The phrase your keyword recognition model triggers on.
                var keyword = "Computer";

                var stopRecognition = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
                var resultStr = "";

                // Creates a speech recognizer using microphone as audio input.
                using (var recognizer = new SpeechRecognizer(_config))
                {
                    // Subscribes to events.
                    recognizer.Recognized += (s, e) =>
                    {
                        if (e.Result.Reason == ResultReason.RecognizedKeyword)
                        {
                            resultStr = $"RECOGNIZED KEYWORD: '{e.Result.Text}'";
                        }
                        else if (e.Result.Reason == ResultReason.RecognizedSpeech)
                        {
                            resultStr = $"RECOGNIZED: '{e.Result.Text}'";
                        }
                        else if (e.Result.Reason == ResultReason.NoMatch)
                        {
                            resultStr = "NOMATCH: Speech could not be recognized.";
                        }
                        Debug.WriteLine(resultStr);
                        //UpdateUI(resultStr);
                    };

                    recognizer.Canceled += (s, e) =>
                    {
                        var cancellation = CancellationDetails.FromResult(e.Result);
                        resultStr = $"CANCELED: Reason={cancellation.Reason} ErrorDetails={cancellation.ErrorDetails}";
                        if (cancellation.Reason == CancellationReason.Error)
                        {
                            // UpdateUI(resultStr);
                        }
                        Debug.WriteLine(resultStr);
                        stopRecognition.TrySetResult(0);
                    };

                    recognizer.SessionStarted += (s, e) =>
                    {
                        Debug.WriteLine("\nSession started event.");
                    };

                    recognizer.SessionStopped += (s, e) =>
                    {
                        Debug.WriteLine("\nSession stopped event.");
                        Debug.WriteLine("\nStop recognition.");
                        stopRecognition.TrySetResult(0);
                    };

                    Debug.WriteLine($"Say something starting with the keyword '{keyword}' followed by whatever you want...");

                    // Starts continuous recognition using the keyword model. Use StopKeywordRecognitionAsync() to stop recognition.
                    await recognizer.StartKeywordRecognitionAsync(model).ConfigureAwait(false);
                    
                    // Waits for a single successful keyword-triggered speech recognition (or error).
                    // Use Task.WaitAny to keep the task rooted.
                    Task.WaitAny(new[] { stopRecognition.Task });

                    await recognizer.StopKeywordRecognitionAsync().ConfigureAwait(false);                  
                }
            }
            catch (Exception ex)
            {
                //UpdateUI("Exception: " + ex.ToString());
            }

        }

        void OutputSpeechRecognitionResult(SpeechRecognitionResult speechRecognitionResult)
        {
            switch (speechRecognitionResult.Reason)
            {
                case ResultReason.RecognizedKeyword:
                    Debug.WriteLine($"RECOGNIZED: Text={speechRecognitionResult.Text} Reason={speechRecognitionResult.Reason}");
                    break;
                case ResultReason.RecognizedSpeech:
                    Debug.WriteLine($"RECOGNIZED: Text={speechRecognitionResult.Text} Reason={speechRecognitionResult.Reason}");
                    break;
                case ResultReason.NoMatch:
                    Debug.WriteLine($"NOMATCH: Speech could not be recognized.");
                    break;
                case ResultReason.Canceled:
                    var cancellation = CancellationDetails.FromResult(speechRecognitionResult);
                    Debug.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Debug.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Debug.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                        Debug.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
                    }
                    break;
            }
        }
    }
}
