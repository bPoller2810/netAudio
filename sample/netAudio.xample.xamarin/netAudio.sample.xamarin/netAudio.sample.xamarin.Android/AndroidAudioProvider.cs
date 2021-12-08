using netAudio.core.Sources;
using netAudio.core.Targets;
using netAudio.droid.Sources;
using netAudio.droid.Targets;

namespace netAudio.sample.xamarin.Droid
{
    public class AndroidAudioProvider : IAudioProvider
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