using netAudio.core.Outputs;
using System;
using System.Collections.Generic;
using System.Text;

namespace netAudio.console.sample
{
    public class ConsoleTarget : IAudioTarget
    {

        #region IAudioTarget
        public void OutputAudioData(byte[] data)
        {
            Console.WriteLine(Encoding.Default.GetString(data));
        }
        public void Open()
        {//do nothing
        }
        public void Close()
        {//do nothing
        }
        #endregion

        #region IDisposable 
        public void Dispose()
        {
            //nothing to dispose here
        }
        #endregion

    }
}
