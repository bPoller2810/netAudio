using MvvMHelpers.core;
using NAudio.CoreAudioApi;
using netAudio.core;
using netAudio.core.Sources;
using netAudio.core.Targets;
using netAudio.win.Sources;
using netCall.win.Targets;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Input;

namespace netAudio.sample.wpf
{
    public class MainViewModel : BaseViewModel
    {
        #region recording
        private AudioPipe? _recordPipe;
        private UdpAudioTarget? _networkTarget;
        private UdpClient? _recordClient;

        private bool _isRecording;
        public bool IsRecording
        {
            get => _isRecording;
            set => Set(ref _isRecording, value);
        }

        public ConditionalObservableCollection<MMDevice> Microphones { get; init; }

        private MMDevice? _selectedMicrophone;
        public MMDevice? SelectedMicrophone
        {
            get => _selectedMicrophone;
            set => Set(ref _selectedMicrophone, value);
        }

        private MicAudioSource? _audioSource;
        public MicAudioSource? AudioSource
        {
            get => _audioSource;
        }

        private string? _remoteIp;
        public string? RemoteIp
        {
            get => _remoteIp;
            set => Set(ref _remoteIp, value);
        }

        private int _remotePort;
        public int RemotePort
        {
            get => _remotePort;
            set => Set(ref _remotePort, value);
        }
        #endregion

        #region playback
        private AudioPipe? _playbackPipe;
        private SpeakerAudioTarget? _audioTarget;
        private UdpClient? _speakerClient;
        private UdpAudioSource? _networkSource;

        private bool _isPlaying;
        public bool IsPlaying
        {
            get => _isPlaying;
            set => Set(ref _isPlaying, value);
        }

        public ConditionalObservableCollection<MMDevice> Speakers { get; init; }

        private MMDevice? _selectedSpeaker;
        public MMDevice? SelectedSpeaker
        {
            get => _selectedSpeaker;
            set => Set(ref _selectedSpeaker, value);
        }

        private int _localPort;
        public int LocalPort
        {
            get => _localPort;
            set => Set(ref _localPort, value);
        }
        #endregion

        #region commands
        public ICommand StartRecordCommand { get; init; }
        public ICommand StopRecordCommand { get; init; }
        public ICommand StartPlaybackCommand { get; init; }
        public ICommand StopPlaybackCommand { get; init; }
        #endregion

        public MainViewModel()
        {
            RemoteIp = "192.168.8.123";
            RemotePort = 8811;
            LocalPort = 8800;

            StartRecordCommand = new AsyncCommand(HandleStartRecord);
            StopRecordCommand = new AsyncCommand(HandleStopRecord);
            StartPlaybackCommand = new AsyncCommand(HandleStartPlayback);
            StopPlaybackCommand = new AsyncCommand(HandleStopPlayback);

            Microphones = new ConditionalObservableCollection<MMDevice>((i) => true);
            Microphones.AddRange(MicAudioSource.GetDevices());

            Speakers = new ConditionalObservableCollection<MMDevice>((i) => true);
            Speakers.AddRange(SpeakerAudioTarget.GetDevices());
        }

        #region command handling
        private Task HandleStopPlayback(object arg)
        {
            _playbackPipe?.Close();
            _playbackPipe = null;
            _networkSource?.Close();
            _networkSource = null;
            _audioTarget?.Close();
            _audioTarget = null;
            _speakerClient?.Close();
            _speakerClient = null;

            return Task.CompletedTask;
        }

        private Task HandleStartPlayback(object arg)
        {
            if (arg is not MMDevice speaker ||
                _audioSource is null ||
                _audioTarget is not null ||
                _speakerClient is not null ||
                _networkSource is not null)
            {
                return Task.CompletedTask;
            }
            _audioTarget = new SpeakerAudioTarget(speaker, _audioSource.WaveFormat);
            _speakerClient = new UdpClient(_localPort);
            var tmpRemote = new IPEndPoint(IPAddress.Parse("127.0.0.1"), _localPort);
            _networkSource = new UdpAudioSource(_speakerClient, tmpRemote);

            _playbackPipe = new AudioPipe(_networkSource, _audioTarget);
            _audioTarget.Open();
            _networkSource.Open();
            _playbackPipe.Open();

            IsPlaying = true;
            return Task.CompletedTask;
        }

        private Task HandleStopRecord(object arg)
        {
            _recordPipe?.Close();
            _recordPipe = null;
            _audioSource?.Close();
            _audioSource = null;
            _networkTarget?.Close();
            _networkTarget = null;
            _recordClient?.Close();
            _recordClient = null;

            IsRecording = false;
            OnPropertyChanged(nameof(AudioSource));
            return Task.CompletedTask;
        }

        private Task HandleStartRecord(object arg)
        {
            if (arg is not MMDevice mic ||
                _recordPipe is not null ||
                _recordClient is not null ||
                _networkTarget is not null ||
                RemoteIp is null)
            {
                return Task.CompletedTask;
            }
            _audioSource = new MicAudioSource(mic);
            _recordClient = new UdpClient();
            var remoteAdress = new IPEndPoint(IPAddress.Parse(RemoteIp), RemotePort);
            _networkTarget = new UdpAudioTarget(_recordClient, remoteAdress);

            _recordPipe = new AudioPipe(_audioSource, _networkTarget);
            _audioSource.Open();
            _networkTarget.Open();
            _recordPipe.Open();

            IsRecording = true;
            OnPropertyChanged(nameof(AudioSource));
            return Task.CompletedTask;
        }
        #endregion

    }
}
