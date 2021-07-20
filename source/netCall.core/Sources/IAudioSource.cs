using System;

namespace netAudio.core.Sources
{

    /// <summary>
    /// The basic definition of a working Audio source
    /// </summary>
    public interface IAudioSource
    {
        /// <summary>
        /// Raised if your source did receive / record Audio
        /// </summary>
        event EventHandler<byte[]> AudioCaptured;

        /// <summary>
        /// Starts forwarding the incomming Audio
        /// </summary>
        /// <returns>true if the action was successfull</returns>
        bool Open();

        /// <summary>
        /// Stops forwarding the incomming Audio
        /// </summary>
        /// <returns>true if the action was successfull</returns>
        bool Close();

    }
}
