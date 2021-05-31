using System;

namespace netCall.core.Outputs
{
    public interface IAudioTarget : IDisposable
    {

        void OutputAudioData(byte[] data);

    }
}
