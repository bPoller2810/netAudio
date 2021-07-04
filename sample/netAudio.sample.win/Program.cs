using netAudio.core;
using netAudio.win.Sources;
using netCall.win.Outputs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace netAudio.sample.win
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Factory.StartNew(() =>
            {
                var sourceDevice = MicAudioSource.GetDevices()
                    .FirstOrDefault(d => d.FriendlyName == "Microphone (Blue Snowball )");

                var targetDevice = SpeakerAudioTarget.GetDevices()
                    .FirstOrDefault(d => d.FriendlyName == "Kopfhörer (2- High Definition Audio Device)");

                var source = new MicAudioSource(sourceDevice);
                var target = new SpeakerAudioTarget(targetDevice, source.WaveFormat);

                var pipe = new AudioPipe(source, target);

                source.Open();
                target.Open();
                pipe.Open();
            });

            Console.ReadLine();


        }
    }
}
