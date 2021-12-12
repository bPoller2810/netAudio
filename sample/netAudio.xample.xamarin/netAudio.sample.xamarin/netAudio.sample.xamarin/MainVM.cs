using GalaSoft.MvvmLight.Ioc;
using MvvMHelpers.core;
using netAudio.core;
using netAudio.core.Sources;
using netAudio.core.Targets;
using netAudio.sample.xamarin;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace App1
{
    public class MainVM : BaseViewModel
    {
        #region private member
        private readonly IAudioProvider _audioProvider;

        private AudioPipe _pipe;
        private IAudioTarget _audioTarget;
        private IAudioSource _audioSource;
        #endregion

        #region properties
        private int _sampleRate;
        public int SampleRate
        {
            get => _sampleRate;
            set => Set(ref _sampleRate, value);
        }

        private bool _isPlaying;
        public bool IsPlaying
        {
            get => _isPlaying;
            set => Set(ref _isPlaying, value);
        }
        #endregion

        #region commands
        public ICommand StartCommand { get; private set; }
        public ICommand StopCommand { get; private set; }
        #endregion

        public MainVM()
        {
            _audioProvider = SimpleIoc.Default.GetInstance<IAudioProvider>();

            SampleRate = 8000;

            StartCommand = new AsyncCommand(HandleStart);
            StopCommand = new AsyncCommand(HandleStop);
        }

        #region command handling
        private Task HandleStop(object arg)
        {
            _pipe?.Close();
            _pipe = null;
            _audioTarget?.Close();
            _audioTarget = null;
            _audioSource?.Close();
            _audioSource = null;

            return Task.CompletedTask;
        }

        private async Task HandleStart(object arg)
        {
            if (_audioSource is not null ||
                _audioTarget is not null ||
                _pipe is not null)
            {
                return;
            }

            if (await Permissions.RequestAsync<Permissions.Microphone>() != PermissionStatus.Granted)
            {
                return;
            }

            _audioTarget = _audioProvider.GetSpeakerTarget(SampleRate);
            _audioSource = _audioProvider.GetMicSource(SampleRate);


            _pipe = new AudioPipe(_audioSource, _audioTarget);
            _audioTarget.Open();
            _audioSource.Open();
            _pipe.Open();

            IsPlaying = true;
            return;
        }
        #endregion

    }
}
