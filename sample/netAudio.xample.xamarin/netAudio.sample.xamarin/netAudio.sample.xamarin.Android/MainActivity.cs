using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using GalaSoft.MvvmLight.Ioc;
using netAudio.core.Targets;
using netAudio.droid.Targets;
using netAudio.core.Sources;
using netAudio.droid.Sources;

namespace netAudio.sample.xamarin.Droid
{
    [Activity(Label = "netAudio.sample.xamarin", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RegisterMic();
            RegisterSpeaker();

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void RegisterSpeaker()
        {
            if (!SimpleIoc.Default.IsRegistered<IAudioTarget>())
            {
                SimpleIoc.Default.Register<IAudioTarget>(() => new SpeakerAudioTarget(44100));
            }
        }
        private void RegisterMic()
        {
            if (!SimpleIoc.Default.IsRegistered<IAudioSource>())
            {
                SimpleIoc.Default.Register<IAudioSource>(() => new MicAudioSource(44100));
            }
        }
    }
}