using System;
using CognitiveServicesSpeechTest.Consts;
using CognitiveServicesSpeechTest.Services;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Intent;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CognitiveServicesSpeechTest.Operations
{
	public class SpeechToTextIntentRecognizerContinuousKeyword
	{
        SpeechConfig _config;

        public SpeechToTextIntentRecognizerContinuousKeyword()
		{
            _config = SpeechConfig.FromSubscription(Consts.AppConsts.CognitiveServicesApiKey, Consts.AppConsts.CognitiveServicesRegion);
        }

        public async Task StartKeywordRecognitionAsync()
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
                using (var recognizer = new IntentRecognizer(_config))
                {

                    recognizer.AddIntent("Take me to floor {floorName}.", "ChangeFloors");
                    recognizer.AddIntent("Go to floor {floorName}.", "ChangeFloors");
                    recognizer.AddIntent("{action} the door.", "OpenCloseDoor");

                    // Subscribes to events.
                    recognizer.Recognized += (s, e) =>
                    {
                        if (e.Result.Reason == ResultReason.RecognizedKeyword)
                        {                            
                            //resultStr = $"RECOGNIZED KEYWORD: '{e.Result.Text}'";
                        }
                        else if (e.Result.Reason == ResultReason.RecognizedSpeech)
                        {
                            if (e.Result.Text.Contains("Cancel"))
                            {
                                Console.WriteLine($"RECOGNIZED: Cancle Recognized Speech");
                                stopRecognition.TrySetResult(0);
                            }
                            //resultStr = $"RECOGNIZED: '{e.Result.Text}'";
                        }
                        else if (e.Result.Reason == ResultReason.NoMatch)
                        {
                            //resultStr = "NOMATCH: Speech could not be recognized.";
                        }
                        OutputSpeechRecognitionResult(e.Result);
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

        void OutputSpeechRecognitionResult(IntentRecognitionResult result)
        {
            string value;
            switch (result.Reason)
            {
                case ResultReason.RecognizedKeyword:
                    Console.WriteLine($"RECOGNIZED KEYWORD: Text= {result.Text}");
                    break;
                case ResultReason.RecognizedSpeech:
                    Console.WriteLine($"RECOGNIZED: Text= {result.Text}");
                    Console.WriteLine($"    Intent not recognized.");
                    break;
                case ResultReason.RecognizedIntent:
                    Console.WriteLine($"RECOGNIZED: Text= {result.Text}");
                    Console.WriteLine($"       Intent Id= {result.IntentId}.");
                    var entities = result.Entities;
                    if (entities.TryGetValue("floorName", out value))
                    {
                        Console.WriteLine($"       FloorName= {value}");
                    }

                    if (entities.TryGetValue("action", out value))
                    {
                        Console.WriteLine($"       Action= {value}");
                    }

                    break;
                case ResultReason.NoMatch:
                    {
                        Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                        var noMatch = NoMatchDetails.FromResult(result);
                        switch (noMatch.Reason)
                        {
                            case NoMatchReason.NotRecognized:
                                Console.WriteLine($"NOMATCH: Speech was detected, but not recognized.");
                                break;
                            case NoMatchReason.InitialSilenceTimeout:
                                Console.WriteLine($"NOMATCH: The start of the audio stream contains only silence, and the service timed out waiting for speech.");
                                break;
                            case NoMatchReason.InitialBabbleTimeout:
                                Console.WriteLine($"NOMATCH: The start of the audio stream contains only noise, and the service timed out waiting for speech.");
                                break;
                            case NoMatchReason.KeywordNotRecognized:
                                Console.WriteLine($"NOMATCH: Keyword not recognized");
                                break;
                        }
                        break;
                    }
                case ResultReason.Canceled:
                    {
                        var cancellation = CancellationDetails.FromResult(result);
                        Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                        if (cancellation.Reason == CancellationReason.Error)
                        {
                            Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                            Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                            Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
                        }
                        break;
                    }
                default:
                    break;
            }

        }
    }
}

