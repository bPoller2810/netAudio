using System;

namespace netCall.core.Processors
{
    public interface IAudioProcessor : IDisposable
    {

        byte[] Process(byte[] data);

    }
}
