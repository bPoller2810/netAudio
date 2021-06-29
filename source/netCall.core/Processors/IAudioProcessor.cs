using System;

namespace netAudio.core.Processors
{
    public interface IAudioProcessor : IDisposable
    {

        byte[] Process(byte[] data);

    }
}
