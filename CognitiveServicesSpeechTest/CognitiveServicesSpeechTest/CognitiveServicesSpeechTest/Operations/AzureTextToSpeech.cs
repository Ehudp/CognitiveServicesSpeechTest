using System;
using CognitiveServicesSpeechTest.Consts;
using Microsoft.CognitiveServices.Speech;
using System.Threading.Tasks;

namespace CognitiveServicesSpeechTest.Operations
{
    public class AzureTextToSpeech
    {
        SpeechConfig config;

        public AzureTextToSpeech()
        {
            config = SpeechConfig.FromSubscription(AppConsts.CognitiveServicesApiKey, AppConsts.CognitiveServicesRegion);
            config.SpeechSynthesisVoiceName = "en-US-JennyMultilingualNeural";
            config.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Raw48Khz16BitMonoPcm);
        }


        public async Task<SpeechSynthesisResult> SpeakTextAsync(string text)
        {
            //_logService.Info(AppConsts.LogTag.Assistant, $"Assistant says the following: {text}");

            SpeechSynthesisResult speechResult;

            using (var synthesizer = new SpeechSynthesizer(config))
            {
                speechResult = await synthesizer.SpeakTextAsync(text);
            }

            return speechResult;

            //using var stream = AudioDataStream.FromResult(speechResult);
            //var audio = AppServices.GetIocDependcy<IAudioProvider>();
            //var buffer = GetBuffer(stream);
            //await audio.PlayFromStream(48000, false, buffer.Item1, buffer.Item2);
        }


    }
}
