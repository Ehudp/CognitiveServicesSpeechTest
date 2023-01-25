﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using CognitiveServicesSpeechTest.Consts;
using CognitiveServicesSpeechTest.Operations;
using CognitiveServicesSpeechTest.Services;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Shapes;

namespace CognitiveServicesSpeechTest.ViewModels
{
	public class ServicesSpeechViewModel: ExtendedBindableObject
    {
        //private readonly AzureSpeechToText _listener;
        //private readonly AzureTextToSpeech _talker;
        private readonly SpeechConfig _config;

        public ServicesSpeechViewModel()
		{
            //_listener = new AzureSpeechToText();
            //_talker = new AzureTextToSpeech();
            _config = SpeechConfig.FromSubscription(AppConsts.CognitiveServicesApiKey, AppConsts.CognitiveServicesRegion);
            _config.SpeechSynthesisVoiceName = "en-US-JennyMultilingualNeural";
            _config.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Raw48Khz16BitMonoPcm);
        }


        private string _macAddress;
        public string MacAddress
        {
            get => _macAddress;
            set => SetProperty(ref _macAddress, value);
        }

        private ICommand _speechToTextCommand;
        public ICommand SpeechToTextCommand
        {
            get
            {
                _speechToTextCommand = _speechToTextCommand ?? new Command(async ()=> await OnSpeechToTextCommand());
                return _speechToTextCommand;
            }
        }

        private ICommand _enableMicrophonetCommand;
        public ICommand EnableMicrophonetCommand
        {
            get
            {
                _enableMicrophonetCommand = _enableMicrophonetCommand ?? new Command(async () => await OnEnableMicrophonetCommand());
                return _enableMicrophonetCommand;
            }
        }

        private async Task OnEnableMicrophonetCommand()
        {
           
            var micService = DependencyService.Get<IMicrophoneService>();
            bool micAccessGranted = await micService.GetPermissionsAsync();
            if (!micAccessGranted)
            {
               // UpdateUI("Please give access to microphone");
            }                      
        }

        private async Task OnSpeechToTextCommand()
        {
            //using (var speechSynthesizer = new SpeechSynthesizer(_config))
            //{
            //    await speechSynthesizer.SpeakTextAsync($"Hello, let's hear how is  well-being and activity today");
            //    await speechSynthesizer.SpeakTextAsync(TalkConsts.Greet);
            //}

            //using var synthesizer = new SpeechSynthesizer(_config, null);
            //var text = $"Hello, let's hear how is  well-being and activity today";
            //var result = await synthesizer.SpeakTextAsync($"Hello, let's hear how is  well-being and activity today");

            //var talker = new AzureTextToSpeech();
            //var result = await talker.SpeakTextAsync($"Hello, let's hear how is  well-being and activity today");
            //Debug.WriteLine(result.Reason);

            //var result = await _talker.SpeakTextAsync(TalkConsts.Greet);
            //Debug.WriteLine(result.Reason);

            await Task.Run(async () =>
            {
                var talker = new AzureTextToSpeech();
                var result = await talker.SpeakTextAsync(TalkConsts.Greet);
                Debug.WriteLine(result.Reason);
            });

            //await Device.InvokeOnMainThreadAsync(async () =>
            //{
            //    var talker = new AzureTextToSpeech();
            //    var result = await talker.SpeakTextAsync($"Hello, let's hear how is  well-being and activity today");

            //});
            //
        }

        private ICommand _textToSpeechCommand;
        public ICommand TextToSpeechCommand
        {
            get
            {
                _textToSpeechCommand = _textToSpeechCommand ?? new Command(async () => await OnTextToSpeechCommand());
                return _textToSpeechCommand;
            }
        }

        private async Task OnTextToSpeechCommand()
        {
            var speechConfig = SpeechConfig.FromSubscription(Consts.AppConsts.CognitiveServicesApiKey, Consts.AppConsts.CognitiveServicesRegion);
            speechConfig.SpeechRecognitionLanguage = "en-US";

            using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

            Console.WriteLine("Speak into your microphone.");
            var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
            OutputSpeechRecognitionResult(speechRecognitionResult);
        }

        private void OnSpeechTextAudioCommand()
        {
            //var config = SpeechConfig.FromSubscription(key, region);
            //using var audioConfig = AudioConfig.FromWavFileOutput(@"C:\repos\AzureCognitiveServices\AzureCognitiveServices\Speech\chapter7-OUT.wav");
            //using var synthesizer = new SpeechSynthesizer(config, audioConfig);
            //await synthesizer.SpeakTextAsync(ExampleText.ChapterExample2);
        }

        static void OutputSpeechRecognitionResult(SpeechRecognitionResult speechRecognitionResult)
        {
            switch (speechRecognitionResult.Reason)
            {
                case ResultReason.RecognizedSpeech:
                    Console.WriteLine($"RECOGNIZED: Text={speechRecognitionResult.Text}");
                    break;
                case ResultReason.NoMatch:
                    Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                    break;
                case ResultReason.Canceled:
                    var cancellation = CancellationDetails.FromResult(speechRecognitionResult);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                        Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
                    }
                    break;
            }
        }
    }
}
