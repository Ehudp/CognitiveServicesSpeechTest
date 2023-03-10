using System;
using CognitiveServicesSpeechTest.Consts;
using Microsoft.CognitiveServices.Speech;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech.Audio;
using System.Diagnostics;

namespace CognitiveServicesSpeechTest.Operations
{
	public class SpeechToTextOnce
	{
        SpeechConfig _speechConfig;

        public SpeechToTextOnce()
        {
            _speechConfig = SpeechConfig.FromSubscription(AppConsts.CognitiveServicesApiKey, AppConsts.CognitiveServicesRegion);            
        }

        //The end of a single utterance is determined by listening for silence at the end or until a maximum of 15 seconds of audio is processed.
        public async Task<SpeechRecognitionResult> RecognizeOnceAsync()
        {
            //can play with timeout
            //https://learn.microsoft.com/en-us/azure/cognitive-services/speech-service/how-to-recognize-speech?pivots=programming-language-csharp#:~:text=speechConfig.SetProperty(PropertyId.-,Speech_SegmentationSilenceTimeoutMs,-%2C%20%222000%22
            //_speechConfig.SetProperty(PropertyId.Speech_SegmentationSilenceTimeoutMs, "300");
            //_speechConfig.SetProperty(PropertyId.SpeechServiceConnection_InitialSilenceTimeoutMs, "10000");

            using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
                using var speechRecognizer = new SpeechRecognizer(_speechConfig, audioConfig);

                var result = await speechRecognizer.RecognizeOnceAsync();
                OutputSpeechRecognitionResult(result);
                return result;

        }

        void OutputSpeechRecognitionResult(SpeechRecognitionResult speechRecognitionResult)
        {
            switch (speechRecognitionResult.Reason)
            {
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

