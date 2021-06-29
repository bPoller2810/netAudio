using netAudio.core.Processors;
using System;
using System.Collections.Generic;
using System.Text;

namespace netAudio.console.sample
{
    public class ToUpperProcessor : IAudioProcessor
    {
       

        #region IAudioProcessor
        public byte[] Process(byte[] data)
        {
            var str = Encoding.Default.GetString(data);
            var upper = str.ToUpper();
            return Encoding.Default.GetBytes(upper);
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {//do nothing
        }
        #endregion

    }
}
