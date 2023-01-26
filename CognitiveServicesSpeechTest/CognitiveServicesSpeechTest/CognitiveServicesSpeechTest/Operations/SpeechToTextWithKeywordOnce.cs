using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CognitiveServicesSpeechTest.Consts;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Xamarin.Forms;

namespace CognitiveServicesSpeechTest.Operations
{
	public class SpeechToTextWithKeywordOnce
	{   
        public async Task<KeywordRecognitionResult> RecognizeOnceAsync()
        {
            var keyword = AppConsts.HeyAnimoKeyWork;
            var kwsModelDir = DependencyService.Get<Services.IAssetService>().GetAssetPath(AppConsts.HeyAnimoFile);
            var keywordModel = KeywordRecognitionModel.FromFile(kwsModelDir);
            using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            using var keywordRecognizer = new KeywordRecognizer(audioConfig);
            Debug.WriteLine($"Say something starting with the keyword '{keyword}' followed by whatever you want...");
            KeywordRecognitionResult result = await keywordRecognizer.RecognizeOnceAsync(keywordModel);
            return result;
        }
	}
}