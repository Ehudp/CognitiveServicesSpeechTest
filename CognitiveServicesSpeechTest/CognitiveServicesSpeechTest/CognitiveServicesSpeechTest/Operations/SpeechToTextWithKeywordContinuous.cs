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
        private readonly SpeechConfig _config;

        public SpeechToTextWithKeywordContinuous()
        {
            _config = SpeechConfig.FromSubscription(AppConsts.CognitiveServicesApiKey, AppConsts.CognitiveServicesRegion);
        }

        public async Task StartContinuousRecognizeAsync()
        {
            try
            {
                var kwsModelDir = DependencyService.Get<IAssetService>().GetAssetPath(AppConsts.HeyAnimoFile);
                var model = KeywordRecognitionModel.FromFile(kwsModelDir);

                // The phrase your keyword recognition model triggers on.                
                var keyword = AppConsts.HeyAnimoKeyWork;

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
    }
}
