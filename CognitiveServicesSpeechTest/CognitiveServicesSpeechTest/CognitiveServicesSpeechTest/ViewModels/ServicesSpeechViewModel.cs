using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using CognitiveServicesSpeechTest.Consts;
using CognitiveServicesSpeechTest.Operations;
using CognitiveServicesSpeechTest.Services;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Shapes;

namespace CognitiveServicesSpeechTest.ViewModels
{
	public class ServicesSpeechViewModel: ExtendedBindableObject
    {
        private readonly SpeechToTextOnce _listener;
        private readonly TextToSpeechOnce _talker;
        private readonly SpeechConfig _config;

        public ServicesSpeechViewModel()
		{
            _listener = new SpeechToTextOnce();
            _talker = new TextToSpeechOnce();

            _config = SpeechConfig.FromSubscription(AppConsts.CognitiveServicesApiKey, AppConsts.CognitiveServicesRegion);
            _config.SpeechSynthesisVoiceName = "en-US-JennyMultilingualNeural";
            _config.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Raw48Khz16BitMonoPcm);
        }


        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
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

        private ICommand _textToSpeechCommand;
        public ICommand TextToSpeechCommand
        {
            get
            {
                _textToSpeechCommand = _textToSpeechCommand ?? new Command(async () => await OnTextToSpeechCommand());
                return _textToSpeechCommand;
            }
        }

        private ICommand _speechToTextOnceCommand;
        public ICommand SpeechToTextOnceCommand
        {
            get
            {
                _speechToTextOnceCommand = _speechToTextOnceCommand ?? new Command(async () => await OnSpeechToTextOnceCommand());
                return _speechToTextOnceCommand;
            }
        }

        private ICommand _speechToTextContinuousCommand;
        public ICommand SpeechToTextContinuousCommand
        {
            get
            {
                _speechToTextContinuousCommand = _speechToTextContinuousCommand ?? new Command(async () => await OnSpeechToTextContinuousCommand());
                return _speechToTextContinuousCommand;
            }
        }


        private ICommand _textToSpeechWithKeywordOnceCommand;
        public ICommand SpeecToTexthWithKeywordOnceCommand
        {
            get
            {
                _textToSpeechWithKeywordOnceCommand = _textToSpeechWithKeywordOnceCommand ?? new Command(async () => await OnSpeecToTexthWithKeywordOnceCommand());
                return _textToSpeechWithKeywordOnceCommand;
            }
        }


        private ICommand _speechToTextWithKeywordContinuousCommand;
        public ICommand SpeechToTextWithKeywordContinuousCommand
        {
            get
            {
                _speechToTextWithKeywordContinuousCommand = _speechToTextWithKeywordContinuousCommand ?? new Command(async () => await OnSpeechToTextWithKeywordContinuousCommand());
                return _speechToTextWithKeywordContinuousCommand;
            }
        }

        private ICommand _speechToTextIntentRecognizerOnceCommand;
        public ICommand SpeechToTextIntentRecognizerOnceCommand
        {
            get
            {
                _speechToTextIntentRecognizerOnceCommand = _speechToTextIntentRecognizerOnceCommand ?? new Command(async () => await OnSpeechToTextIntentRecognizerOnceCommand());
                return _speechToTextIntentRecognizerOnceCommand;
            }
        }

        private ICommand _speechToTextIntentRecognizerContinuousCommand;
        public ICommand SpeechToTextIntentRecognizerContinuousCommand
        {
            get
            {
                _speechToTextIntentRecognizerContinuousCommand = _speechToTextIntentRecognizerContinuousCommand ?? new Command(async () => await OnSpeechToTextIntentRecognizerContinuousCommand());
                return _speechToTextIntentRecognizerContinuousCommand;
            }
        }

        private ICommand _speechToTextIntentRecognizerContinuousKeywordCommand;
        public ICommand SpeechToTextIntentRecognizerContinuousKeywordCommand
        {
            get
            {
                _speechToTextIntentRecognizerContinuousKeywordCommand = _speechToTextIntentRecognizerContinuousKeywordCommand ?? new Command(async () => await OnSpeechToTextIntentRecognizerContinuousKeywordCommand());
                return _speechToTextIntentRecognizerContinuousKeywordCommand;
            }
        }

        private ICommand _speechToTextIntentContinuousKeywordConversationCommand;
        public ICommand SpeechToTextIntentContinuousKeywordConversationCommand
        {
            get
            {
                _speechToTextIntentContinuousKeywordConversationCommand = _speechToTextIntentContinuousKeywordConversationCommand ?? new Command(async () => await OnSpeechToTextIntentContinuousKeywordConversationCommand());
                return _speechToTextIntentContinuousKeywordConversationCommand;
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
        
        private async Task OnTextToSpeechCommand()
        {
            var text = $"Hello, let's hear how is  well-being and activity today";
            await _talker.SpeakTextAsync(text);
            var result = await _talker.SpeakTextAsync(TalkConsts.Greet);
            Debug.WriteLine(result.Reason);

            //using (var speechSynthesizer = new SpeechSynthesizer(_config))
            //{
            //    await speechSynthesizer.SpeakTextAsync($"Hello, let's hear how is  well-being and activity today");
            //    await speechSynthesizer.SpeakTextAsync(TalkConsts.Greet);
            //}
        }


        private async Task OnSpeechToTextWithKeywordContinuousCommand()
        {
            var listener = new SpeechToTextWithKeywordContinuous();
            await listener.StartContinuousRecognizeAsync();
        }

        private async Task OnSpeechToTextContinuousCommand()
        {
            var listener = new SpeechToTextContinuous();
            var result = await listener.StartContinuousRecognizeAsync();         
        }


        private async Task OnSpeechToTextIntentRecognizerOnceCommand()
        {
            var listener = new SpeechToTextIntentRecognizerOnce();
            var result = await listener.RecognizeOnceAsync();
        }


        private async Task OnSpeechToTextIntentRecognizerContinuousCommand()
        {
            var listener = new SpeechToTextIntentRecognizerContinuous();
            await listener.StartContinuousRecognitionAsync();
        }


        private async Task OnSpeechToTextIntentRecognizerContinuousKeywordCommand()
        {
            var listener = new SpeechToTextIntentRecognizerContinuousKeyword();
            await listener.StartKeywordRecognitionAsync();
        }


        private async Task OnSpeechToTextIntentContinuousKeywordConversationCommand()
        {
            var listener = new SpeechToTextIntentContinuousKeywordConversation();
            await listener.StartKeywordRecognitionAsync();
        }

        private async Task OnSpeechToTextOnceCommand()
        {
            var result = await _listener.RecognizeOnceAsync();
        }

        private async Task OnSpeecToTexthWithKeywordOnceCommand()
        {
            var listener = new SpeechToTextWithKeywordOnce();
            var result = await listener.RecognizeOnceAsync();
            Debug.WriteLine($"Text={result.Text} Reason={result.Reason}");
        }


        private void OnSpeechTextAudioCommand()
        {

            //var config = SpeechConfig.FromSubscription(key, region);
            //using var audioConfig = AudioConfig.FromWavFileOutput(@"C:\repos\AzureCognitiveServices\AzureCognitiveServices\Speech\chapter7-OUT.wav");
            //using var synthesizer = new SpeechSynthesizer(config, audioConfig);
            //await synthesizer.SpeakTextAsync(ExampleText.ChapterExample2);
        }

       
    }
}

