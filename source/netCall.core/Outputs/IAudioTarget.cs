using System;

namespace netAudio.core.Outputs
{
    public interface IAudioTarget : IDisposable
    {

        void OutputAudioData(byte[] data);

        bool Open();
        bool Close();

    }
}
