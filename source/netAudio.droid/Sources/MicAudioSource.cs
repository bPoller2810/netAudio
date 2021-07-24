using Android.Media;
using netAudio.core.Sources;
using System;
using System.Linq;
using System.Threading.Tasks;
using Encoding = Android.Media.Encoding;

namespace netAudio.droid.Sources
{
    public class MicAudioSource : IAudioSource, IDisposable
    {

        #region private member
        private readonly AudioRecord _recorder;
        private readonly byte[] _audioBuffer;
        private Task _recordTask;
        private bool _working;
        #endregion

        #region ctor
        public MicAudioSource(
            int sampleRate,
            AudioSource source = AudioSource.Mic,
            Encoding encoding = Encoding.Pcm16bit,
            ChannelIn channel = ChannelIn.Mono)
        {
            var minBufferSize = AudioRecord.GetMinBufferSize(sampleRate, ChannelIn.Mono, encoding);
            _audioBuffer = new byte[minBufferSize];

            _recorder = new AudioRecord(
                source,
                sampleRate,
                channel,
                encoding,
                minBufferSize);

            //TODO: use Builder. WARNING: SetChannelMask requires ChannelOut wich generates an invalid Record
            //_recorder = new AudioRecord.Builder()
            //       .SetAudioSource(source)
            //       .SetAudioFormat(new AudioFormat.Builder()
            //           .SetSampleRate(sampleRate)
            //           .SetEncoding(encoding)
            //           .SetChannelMask(channel)
            //           .Build())
            //       .SetBufferSizeInBytes(minBufferSize)
            //       .Build();

        }
        #endregion

        #region private task
        private async Task RecordAudioAsync()
        {
            _recorder.StartRecording();
            while (_working)
            {
                try
                {
                    var count = await _recorder.ReadAsync(_audioBuffer, 0, _audioBuffer.Length);
                    if (count == 0)
                    {
                        await Task.Delay(1);
                        continue;
                    }
                    AudioCaptured?.Invoke(this, _audioBuffer.Take(count).ToArray());
                }
                catch (Exception ex)
                {
                    OnError?.Invoke(this, ex);
                }
            }

            _recorder.Stop();
            _recorder.Release();
        }
        #endregion

        #region IAudioSource
        public event EventHandler<Exception> OnError;
        public event EventHandler<byte[]> AudioCaptured;
        public bool Open()
        {
            if (_working)
            {
                return false;
            }
            _working = true;
            _recordTask = Task.Run(RecordAudioAsync);

            return true;
        }
        public bool Close()
        {
            if (!_working)
            {
                return false;
            }
            _working = false;
            _recordTask.Wait();
            return true;
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            _recorder.Release();
        }
        #endregion

    }
}