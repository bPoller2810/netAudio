using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using netAudio.core.Enumerations;
using netAudio.core.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encoding = Android.Media.Encoding;

namespace netAudio.droid.Sources
{
    public class DroidMicSource : IAudioSource
    {

        #region private member
        private readonly AudioRecord _recorder;
        private readonly byte[] _audioBuffer;
        #endregion

        #region properties
        public bool IsOpen { get; private set; }
        #endregion

        #region ctor
        public DroidMicSource(int sampleRate, AudioEncoding encoding)
        {
            var minBufferSize = AudioRecord.GetMinBufferSize(sampleRate, ChannelIn.Mono, GetEncoding(encoding));
            _audioBuffer = new byte[minBufferSize];
            _recorder = new AudioRecord(
                AudioSource.Mic,
                sampleRate,
                ChannelIn.Mono,
                GetEncoding(encoding),
                _audioBuffer.Length);
        }
        #endregion

        #region private helper
        private Encoding GetEncoding(AudioEncoding encoding)
        {
            return encoding switch
            {
                AudioEncoding.Pcm16bit => Encoding.Pcm16bit,

                _ => throw new NotImplementedException($"{encoding} not implemented"),
            };
        }
        #endregion

        #region IAudioSource
        public event EventHandler<byte[]> AudioCaptured;
        public void Open()
        {
            if (IsOpen)
            {
                throw new InvalidOperationException("Source is already open");
            }
            IsOpen = true;
            _recorder.StartRecording();
            //TODO: start receive task
        }
        public void Close()
        {
            if (!IsOpen)
            {
                throw new InvalidOperationException("Source is already closed");
            }
            IsOpen = false;
            _recorder.Stop();
            //TODO: stop receive task
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