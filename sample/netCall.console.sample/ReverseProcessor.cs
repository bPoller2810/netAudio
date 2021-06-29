using netAudio.core.Processors;
using System;
using System.Collections.Generic;
using System.Text;

namespace netAudio.console.sample
{
    public class ReverseProcessor : IAudioProcessor
    {
        #region IAudioProcessor
        public byte[] Process(byte[] data)
        {
            Array.Reverse(data);
            return data;
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {//do nothing
        }
        #endregion
    }
}
