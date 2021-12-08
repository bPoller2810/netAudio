using netAudio.core.Sources;
using netAudio.core.Targets;
using System;
using System.Collections.Generic;
using System.Text;

namespace netAudio.sample.xamarin
{
    public interface IAudioProvider
    {
        IAudioSource GetMicSource(int sampleRate);
        IAudioTarget GetSpeakerTarget(int sampleRate);

    }
}
