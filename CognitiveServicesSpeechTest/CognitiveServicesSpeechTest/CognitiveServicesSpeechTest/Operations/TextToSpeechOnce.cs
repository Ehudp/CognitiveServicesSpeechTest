using System;
using CognitiveServicesSpeechTest.Consts;
using Microsoft.CognitiveServices.Speech;
using System.Threading.Tasks;

namespace CognitiveServicesSpeechTest.Operations
{
    public class TextToSpeechOnce
    {
        SpeechConfig config;

        public TextToSpeechOnce()
        {
            config = SpeechConfig.FromSubscription(AppConsts.CognitiveServicesApiKey, AppConsts.CognitiveServicesRegion);
            config.SpeechSynthesisVoiceName = "en-US-JennyMultilingualNeural";
            config.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Raw48Khz16BitMonoPcm);
        }


        public async Task<SpeechSynthesisResult> SpeakTextAsync(string text)
        {            
            SpeechSynthesisResult speechResult;

            using (var synthesizer = new SpeechSynthesizer(config))
            {
                speechResult = await synthesizer.SpeakTextAsync(text);
                return speechResult;
            }           
        }
    }
}
