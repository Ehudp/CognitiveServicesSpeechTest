using System;
using CognitiveServicesSpeechTest.Consts;
using Microsoft.CognitiveServices.Speech;
using System.Threading.Tasks;

namespace CognitiveServicesSpeechTest.Operations
{
	public class AzureSpeechToText
	{
        //public async Task<string> Recognize()
        //{

        //    try
        //    {
        //        var path = AppServices.GetIocDependcy<IPathOSProvider>().GetPath("azure");
        //        AppServices.GetIocDependcy<IPathOSProvider>().CreateDir(path);
        //        var filePath = $"{path}/speechToText.txt";


        //        var config = SpeechConfig.FromSubscription(AppConsts.CognitiveServicesApiKey,
        //                AppConsts.CognitiveServicesRegion);
        //        using var recognizer = new SpeechRecognizer(config);
        //        config.SetProperty(PropertyId.Speech_LogFilename, filePath);
        //        var sound = Mvx.IoCProvider.Resolve<IAudioProvider>();
        //        sound.PlaySound(AppConsts.Audio.SuccessSound);
        //        var result = await recognizer.RecognizeOnceAsync();
        //        var newMessage = string.Empty;
        //        if (result.Reason == ResultReason.RecognizedSpeech)
        //        {
        //            newMessage = result.Text;
        //        }
        //        else if (result.Reason == ResultReason.NoMatch)
        //        {
        //            newMessage = "NOMATCH: Speech could not be recognized.";
        //        }
        //        else if (result.Reason == ResultReason.Canceled)
        //        {
        //            var cancellation = CancellationDetails.FromResult(result);
        //            newMessage = $"CANCELED: Reason={cancellation.Reason} ErrorDetails={cancellation.ErrorDetails}";
        //        }

        //        return newMessage;


        //    }
        //    catch (Exception ex)
        //    {
        //        AppServices.GetIocDependcy<ILogService>().HandleException(ex);
        //    }

        //    return string.Empty;
        //}

        //private string GetRecognitionResult(SpeechRecognitionResult result) => result.Reason switch
        //{
        //    ResultReason.RecognizedSpeech => result.Text,
        //    _ => string.Empty
        //};
    }
}

