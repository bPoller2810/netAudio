using netAudio.core.Sources;
using System;
using System.Text;

namespace netAudio.console.sample
{
    public class DemoSource : IAudioSource
    {
        #region private member
        private readonly string _data;
        #endregion

        #region ctor
        public DemoSource(string data)
        {
            _data = data;
        }
        #endregion

        #region IAudioSource
        public event EventHandler<byte[]> AudioCaptured;

        public void Open()
        {// send the ctor argument string data once
            AudioCaptured?.Invoke(this, Encoding.Default.GetBytes(_data));
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
