using NAudio.Wave;
using netAudio.core.Sources;
using System;
using System.Collections.Generic;
using netAudio.core.Extensions;
using NAudio.CoreAudioApi;

namespace netAudio.win.Sources
{
    public class MicAudioSource : IAudioSource
    {
        #region private member
        private bool _open;

        private readonly WasapiCapture _audioSource;
        #endregion

        #region properties
        public WaveFormat WaveFormat=> _audioSource.WaveFormat;
        #endregion

        #region ctor
        public MicAudioSource(MMDevice sourceDevice)
        {
            _audioSource = new WasapiCapture(sourceDevice);
            _audioSource.DataAvailable += HandleAudioSourced;
        }
        #endregion

        #region public helper
        public static IEnumerable<MMDevice> GetDevices()
        {
            var enumerator = new MMDeviceEnumerator();
            return enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
        }
        #endregion

        #region event handling
        private void HandleAudioSourced(object sender, WaveInEventArgs args)
        {
            AudioCaptured?.Invoke(this, args.Buffer.SubArray(0, args.BytesRecorded));
        }
        #endregion

        #region IAudioSource
        public event EventHandler<byte[]> AudioCaptured;

        public bool Open()
        {
            if(_open)
            {
                return false;
            }
            _audioSource.DataAvailable += HandleAudioSourced;
            _audioSource.StartRecording();
            return true;
        }
        public bool Close()
        {
            if(!_open)
            {
                return false;
            }
            _audioSource.DataAvailable -= HandleAudioSourced;
            _audioSource.StopRecording();
            return true;
        }

        #endregion

        #region IDisposable
        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Close();
                    _audioSource.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
