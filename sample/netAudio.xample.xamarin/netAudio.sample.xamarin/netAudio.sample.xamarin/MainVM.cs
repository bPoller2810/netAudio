using GalaSoft.MvvmLight.Ioc;
using netAudio.core;
using netAudio.core.Sources;
using netAudio.core.Targets;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App1
{
    public class MainVM : INotifyPropertyChanged
    {
        private IAudioTarget _speaker;
        private IAudioSource _mic;
        private AudioPipe _pipe;


        private bool _inCall;
        public bool InCall
        {
            get => _inCall;
            set
            {

                _inCall = value;
                OnPropertyChanged();
                ((Command)StartCall).ChangeCanExecute();
                ((Command)StopCall).ChangeCanExecute();

            }
        }


        public ICommand StartCall { get; private set; }
        public ICommand StopCall { get; private set; }

        public MainVM()
        {
            StartCall = new Command(ExecuteStartCall, CanStartCall);
            StopCall = new Command(ExecuteStopCall, CanStopCall);
        }

        private bool CanStopCall()
        {
            return InCall;
        }
        private bool CanStartCall()
        {
            return !InCall;
        }

        private void ExecuteStartCall()
        {
            //Permissions.RequestAsync<Permissions.Microphone>().Wait();

            InCall = true;
            _speaker = SimpleIoc.Default.GetInstanceWithoutCaching<IAudioTarget>();
            _mic = SimpleIoc.Default.GetInstanceWithoutCaching<IAudioSource>();
            _pipe = new AudioPipe(_mic, _speaker);
            _speaker.Open();
            _mic.Open();
            _pipe.Open();
        }
        private void ExecuteStopCall()
        {
            InCall = false;
            _speaker.Close();
            _mic.Close();
            _pipe.Close();
            if (_speaker is IDisposable disposableSpeaker)
            {
                disposableSpeaker.Dispose();
            }
            if (_mic is IDisposable disposableMic)
            {
                disposableMic.Dispose();
            }
            _speaker = null;
            _mic = null;
            _pipe = null;
        }



        protected void OnPropertyChanged([CallerMemberName] string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
