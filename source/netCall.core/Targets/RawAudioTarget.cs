using System;

namespace netAudio.core.Targets
{

    /// <summary>
    /// A Target to hook into other Systems
    /// Subscribe to AudioOutput to access the audio stream
    /// </summary>
    public class RawAudioTarget : IAudioTarget
    {
        #region events
        public event EventHandler<byte[]> AudioOutput;
        #endregion

        #region private member
        private bool _isOpen;
        #endregion

        #region IAudioTarget
        public bool Open()
        {
            _isOpen = true;
            return true;
        }
        public bool Close()
        {
            _isOpen = false;
            return true;
        }
        public void OutputAudioData(byte[] data)
        {
            if (!_isOpen)
            {
                return;
            }
            AudioOutput?.Invoke(this, data);
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {//nothing to dispose
        }
        #endregion
    }
}
