using System;

namespace netAudio.core.Sources
{
    public interface IAudioSource : IDisposable
    {

        event EventHandler<byte[]> AudioCaptured;

        void Open();
        void Close();

    }
}
