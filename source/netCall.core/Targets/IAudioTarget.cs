using System;

namespace netAudio.core.Targets
{

    /// <summary>
    /// The basic definition of a working Audio Target / Output
    /// </summary>
    public interface IAudioTarget
    {
        /// <summary>
        /// Takes the Audio to be Played or Processed
        /// </summary>
        /// <param name="data">The audio Data</param>
        void OutputAudioData(byte[] data);

        /// <summary>
        /// Starts forwarding the audio to the Output device
        /// </summary>
        /// <returns>true if the action was successfull</returns>
        bool Open();

        /// <summary>
        /// Stops forwarding the audio to the Output device
        /// </summary>
        /// <returns>true if the action was successfull</returns>
        bool Close();

    }
}
