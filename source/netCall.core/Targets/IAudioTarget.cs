using System;

namespace netAudio.core.Targets
{
    public interface IAudioTarget : IDisposable
    {

        void OutputAudioData(byte[] data);

        bool Open();
        bool Close();

    }
}
