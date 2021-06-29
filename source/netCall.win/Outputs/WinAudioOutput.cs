using NAudio.Wave;
using netCall.core.Outputs;
using netCall.win.Extensions;
using netCall.win.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace netCall.win.Outputs
{
    public class WinAudioOutput //: IAudioTarget
    {

        #region private member
        private readonly int _deviceNumber;
        #endregion

        #region ctor
        public WinAudioOutput(int deviceNumber)
        {
            _deviceNumber = deviceNumber;
        }
        #endregion

        #region public helper
        public static IEnumerable<WinAudioDevice> GetDevices()
        {
            var count = WaveOut.DeviceCount;
            for (int i = 0; i < count; i++)
            {
                var device = WaveOut.GetCapabilities(i);
                yield return new WinAudioDevice(i, device.ProductName);
            }
        }
        #endregion

        #region IAudioOutput
      
      

        public void PlayAudioData(byte[] data)
        {
            var dataStream = new MemoryStream();
            var wo = new WaveOutEvent();
            
            var provider = new RawSourceWaveStream(dataStream);
            wo.Init(provider);
            wo.Play();

        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
