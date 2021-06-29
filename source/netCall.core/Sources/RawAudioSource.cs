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
        public void SourceAudio(byte[] data)
        {
            if (!_isOpen)
            {
                return;
            }
            AudioCaptured?.Invoke(this, data);
        }
        #endregion

        #region IAudioSource
        public event EventHandler<byte[]> AudioCaptured;
        public void Open()
        {
            _isOpen = true;
        }
        public void Close()
        {
            _isOpen = false;
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {//nothing to dispose
        }
        #endregion

    }
}
