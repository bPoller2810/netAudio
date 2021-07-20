using netAudio.core.Extensions;
using System;
using System.Net.Sockets;
using System.Threading;

namespace netAudio.core.Sources
{

    /// <summary>
    /// A Source that receives Data from the Socket
    /// </summary>
    public class SocketAudioSource : IAudioSource
    {
        #region private member
        private readonly Socket _client;

        private Thread _receiveWorker;
        private bool _working;
        #endregion

        #region ctor
        /// <summary>
        /// Creates the Audio Target that receives the data from the Socket
        /// </summary>
        /// <param name="client">The open and ready to receive Socket used to communicate</param>
        public SocketAudioSource(Socket client)
        {
            _client = client;
        }
        #endregion

        #region buffer worker
        private void ReceiveWorker()
        {
            var buffer = new byte[_client.ReceiveBufferSize];
            while (_working)
            {
                if (_client.Available == 0)
                {//the queue seems empty, we just wait until we have data
                    Thread.Sleep(10);
                    continue;
                }

                var received = _client.Receive(buffer);
                AudioCaptured?.Invoke(this, buffer.SubArray(0, received));
            }
        }
        #endregion

        #region IAudioSource
        public event EventHandler<byte[]> AudioCaptured;

        public bool Open()
        {
            if (_working)
            {
                return false;
            }

            _working = true;
            _receiveWorker = new Thread(ReceiveWorker);
            _receiveWorker.Start();
            return true;
        }
        public bool Close()
        {
            if (!_working)
            {
                return false;
            }

            _working = false;
            _receiveWorker?.Join();
            _receiveWorker = null;
            return true;
        }
        #endregion;

    }
}
