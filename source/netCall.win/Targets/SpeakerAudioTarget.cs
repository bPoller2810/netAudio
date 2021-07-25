using NAudio.CoreAudioApi;
using NAudio.Wave;
using netAudio.core.Targets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace netCall.win.Targets
{
    public class SpeakerAudioTarget : IAudioTarget, IDisposable
    {
        #region private member
        private readonly WasapiOut _targetDevice;
        private readonly BufferedWaveProvider _provider;
        private readonly ConcurrentQueue<byte[]> _workerQueue;

        private Thread _bufferWorker;
        private bool _working;
        #endregion

        #region ctor
        public SpeakerAudioTarget(MMDevice targetDevice, WaveFormat waveFormat)
        {
            _workerQueue = new();

            _provider = new BufferedWaveProvider(waveFormat);
            _targetDevice = new WasapiOut(targetDevice, AudioClientShareMode.Shared, true, 200);
            _targetDevice.Init(_provider);
        }
        #endregion

        #region public helper
        public static IEnumerable<MMDevice> GetDevices()
        {
            var enumerator = new MMDeviceEnumerator();
            return enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
        }
        #endregion

        #region buffer worker
        private void BufferWorker()
        {
            while (_working)
            {
                try
                {
                    if (!_workerQueue.TryDequeue(out var data))
                    {//the queue seems empty, we just wait until we have data
                        Thread.Sleep(10);
                        continue;
                    }

                    _provider.ClearBuffer();
                    _provider.AddSamples(data, 0, data.Length);
                }
                catch(Exception ex)
                {
                    OnError?.Invoke(this, ex);
                }
            }
        }
        #endregion

        #region IAudioTarget
        public event EventHandler<Exception> OnError;

        public bool Open()
        {
            if (_working)
            {
                return false;
            }
            _working = true;
            _targetDevice.Play();

            _bufferWorker = new Thread(BufferWorker);
            _bufferWorker.Start();
            return true;
        }
        public bool Close()
        {
            if (!_working)
            {
                return false;
            }
            _targetDevice.Stop();

            _working = false;
            _bufferWorker?.Join();
            _bufferWorker = null;
            return true;
        }
        public void OutputAudioData(byte[] data)
        {
            _workerQueue.Enqueue(data);
        }
        #endregion

        #region IDisposable
        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Close();
                    _targetDevice.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
