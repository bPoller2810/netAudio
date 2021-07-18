using System;

namespace netAudio.core.Targets
{
    public interface IAudioTarget
    {

        void OutputAudioData(byte[] data);

        bool Open();
        bool Close();

    }
}
