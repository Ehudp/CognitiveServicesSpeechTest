using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using CognitiveServicesSpeechTest.Droid.Services;
using CognitiveServicesSpeechTest.Services;

namespace CognitiveServicesSpeechTest.Droid
{
    [Activity(Label = "CognitiveServicesSpeechTest", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private const int RECORD_AUDIO = 1;

        private IMicrophoneService micService;

        internal static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Instance = this;
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());


            Xamarin.Forms.DependencyService.Register<IMicrophoneService, MicrophoneService>();
            micService = Xamarin.Forms.DependencyService.Get<IMicrophoneService>();
            Xamarin.Forms.DependencyService.Register<IAssetService, AssetService>();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            switch (requestCode)
            {
                case RECORD_AUDIO:
                    {
                        if (grantResults[0] == Permission.Granted)
                        {
                            micService.OnRequestPermissionsResult(true);
                        }
                        else
                        {
                            micService.OnRequestPermissionsResult(false);
                        }
                    }
                    break;
            }
        }
    }
}
