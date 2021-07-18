using System;

namespace netAudio.core.Sources
{
    public interface IAudioSource
    {

        event EventHandler<byte[]> AudioCaptured;

        bool Open();
        bool Close();

    }
}
