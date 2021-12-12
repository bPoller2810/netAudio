using System;
using netAudio.core.Targets;

namespace netAudio.sample.xamarin.iOS
{
    public class SpeakerAudioTarget : IAudioTarget
    {
        public SpeakerAudioTarget(int sampleRate)
        {
        }

        #region IAudioTarget
        public event EventHandler<Exception> OnError;

        public bool Open()
        {
            throw new NotImplementedException();
        }

        public bool Close()
        {
            throw new NotImplementedException();
        }

        public void OutputAudioData(byte[] data)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
