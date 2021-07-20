
namespace netAudio.core.Processors
{

    /// <summary>
    /// The basic definition of a AudioProcessor
    /// </summary>
    public interface IAudioProcessor
    {

        /// <summary>
        /// The processing method that should return the modified data as fast as possible
        /// Delays here result in delayed output
        /// </summary>
        /// <param name="data">The incomming data</param>
        /// <returns>The modified data</returns>
        byte[] Process(byte[] data);

    }
}
