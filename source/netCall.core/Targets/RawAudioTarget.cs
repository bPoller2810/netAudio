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
        /// <summary>
        /// Raised if there is oudio to handle in some way
        /// </summary>
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

    }
}
