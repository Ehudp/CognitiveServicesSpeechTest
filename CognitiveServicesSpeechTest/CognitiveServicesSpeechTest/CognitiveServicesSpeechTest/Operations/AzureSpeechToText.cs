using System;
using CognitiveServicesSpeechTest.Consts;
using Microsoft.CognitiveServices.Speech;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech.Audio;
using System.Diagnostics;

namespace CognitiveServicesSpeechTest.Operations
{
	public class AzureSpeechToText
	{
        SpeechConfig _speechConfig;

        public AzureSpeechToText()
        {
            _speechConfig = SpeechConfig.FromSubscription(AppConsts.CognitiveServicesApiKey, AppConsts.CognitiveServicesRegion);            
        }

        public async Task<SpeechRecognitionResult> RecognizeOnceAsync()
        {

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

