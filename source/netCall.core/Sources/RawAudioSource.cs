using System;

namespace netAudio.core.Sources
{

    /// <summary>
    /// A Source to hook into other Systems
    /// Call SourceAudio to Source your Audio into the pipe
    /// </summary>
    public class RawAudioSource : IAudioSource
    {
        #region private member
        private bool _isOpen;
        #endregion

        #region public interaction
        public bool SourceAudio(byte[] data)
        {
            if (!_isOpen)
            {
                return false;
            }
            AudioCaptured?.Invoke(this, data);
            return true;
        }
        #endregion

        #region IAudioSource
        public event EventHandler<byte[]> AudioCaptured;
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
        #endregion

        #region IDisposable
        public void Dispose()
        {//nothing to dispose
        }
        #endregion
    }
}
