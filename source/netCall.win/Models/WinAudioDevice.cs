using System;
using System.Collections.Generic;
using System.Text;

namespace netCall.win.Models
{
    public readonly struct WinAudioDevice
    {

        public int Number { get; }
        public string Name { get; }

        public WinAudioDevice(int number, string name)
        {
            Number = number;
            Name = name;
        }

    }
}
