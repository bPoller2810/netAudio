using netAudio.core.Sources;
using netAudio.core.Targets;

namespace netAudio.sample.xamarin.iOS
{
    public class IosAudioProvider : IAudioProvider
    {
        public IAudioSource GetMicSource(int sampleRate)
        {
            return new MicAudioSource(sampleRate);
        }

        public IAudioTarget GetSpeakerTarget(int sampleRate)
        {
            return new SpeakerAudioTarget(sampleRate);
        }
    }
}
