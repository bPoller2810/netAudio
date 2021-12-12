using System;
using netAudio.core.Sources;

namespace netAudio.sample.xamarin.iOS
{
    public class MicAudioSource : IAudioSource
    {
        public MicAudioSource(int sampleRate)
        {
        }

        #region IAudioSource
        public event EventHandler<Exception> OnError;
        public event EventHandler<byte[]> AudioCaptured;

        public bool Open()
        {
            throw new NotImplementedException();
        }

        public bool Close()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
