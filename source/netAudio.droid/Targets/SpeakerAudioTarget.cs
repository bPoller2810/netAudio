using Android.Media;
using netAudio.core.Targets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Encoding = Android.Media.Encoding;

namespace netAudio.droid.Targets
{
    public class SpeakerAudioTarget : IAudioTarget
    {
        #region private member
        private readonly AudioTrack _targetDevice;

        private readonly Queue<byte[]> _workerQueue;
        private Task _bufferWorker;
        private bool _working;
        #endregion

        #region ctor
        public SpeakerAudioTarget(
            int sampleRate, 
            Encoding encoding = Encoding.Pcm16bit,
            ChannelOut channel = ChannelOut.Mono)
        {
            _workerQueue = new Queue<byte[]>();

            var minBufferSize = AudioTrack.GetMinBufferSize(sampleRate, channel, encoding);

            _targetDevice = new AudioTrack(
                Stream.VoiceCall,
                sampleRate,
                channel,
                encoding,
                minBufferSize,
                AudioTrackMode.Stream);

            //TODO: use Builder. WARNING: this messes with the incomming raw data sample (wrong samplerate)
            //_targetDevice = new AudioTrack.Builder()
            //    .SetAudioAttributes(new AudioAttributes.Builder()
            //        .SetUsage(AudioUsageKind.VoiceCommunication)
            //        .SetContentType(AudioContentType.)
            //        .Build())
            //    .SetAudioFormat(new AudioFormat.Builder()
            //        .SetEncoding(encoding)
            //        .SetSampleRate(sampleRate)
            //        .Build())
            //    .SetBufferSizeInBytes(minBufferSize)
            //    .Build();

        }
        #endregion

        #region private task
        private async Task PlayTask()
        {
            _targetDevice.Play();
            while (_working)
            {
                try
                {
                    if (_workerQueue.Count == 0)
                    {
                        await Task.Delay(5);
                        continue;
                    }

                    var data = _workerQueue.Dequeue();
                    await _targetDevice.WriteAsync(data, 0, data.Length);

                }
                catch (Exception ex)
                {
                    OnError?.Invoke(this, ex);
                }
            }
            _targetDevice.Stop();
        }
        #endregion

        #region IAudioTarget
        public event EventHandler<Exception> OnError;

        public bool Open()
        {
            if (_working)
            {
                return false;
            }
            _working = true;

            _bufferWorker = Task.Run(PlayTask);

            return true;
        }

        public bool Close()
        {
            if (!_working)
            {
                return false;
            }
            _working = false;
            _bufferWorker?.Wait();
            _bufferWorker = null;
            return true;
        }

        public void OutputAudioData(byte[] data)
        {
            _workerQueue.Enqueue(data);
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
                    _targetDevice.Dispose();
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