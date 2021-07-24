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
        /// <summary>
        /// Sends Data into this Source
        /// </summary>
        /// <param name="data">The audio Data to be handled</param>
        /// <returns>true if the action </returns>
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
        public event EventHandler<Exception> OnError;
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

    }
}
