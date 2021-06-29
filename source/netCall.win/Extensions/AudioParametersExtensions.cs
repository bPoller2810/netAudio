using NAudio.Wave;
using netCall.core.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace netCall.win.Extensions
{
    public static class AudioParametersExtensions
    {

        public static WaveFormat ToWaveFormat(this AudioParameters self)
        {
            return new WaveFormat(self.Rate, self.Bits, self.Channels);
        }

    }
}
