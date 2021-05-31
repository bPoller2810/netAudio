using System;

namespace netCall.core.Sources
{
    public interface IAudioSource : IDisposable
    {

        event EventHandler<byte[]> AudioCaptured;

        void Activate();
        void Deactivate();

    }
}
