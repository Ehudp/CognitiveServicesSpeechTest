using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Xamarin.Forms;

namespace CognitiveServicesSpeechTest.Operations
{
	public class SpeechToTextWithKeywordOnce
	{
        private string kwsModelFile = "kws.table";

        public SpeechToTextWithKeywordOnce()
		{
		}

        public async Task<KeywordRecognitionResult> RecognizeOnceAsync()
        {
            var kwsModelDir = DependencyService.Get<Services.IAssetService>().GetAssetPath(kwsModelFile);
            var keywordModel = KeywordRecognitionModel.FromFile(kwsModelDir);
            using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            using var keywordRecognizer = new KeywordRecognizer(audioConfig);
            KeywordRecognitionResult result = await keywordRecognizer.RecognizeOnceAsync(keywordModel);
            return result;
        }
	}
}

