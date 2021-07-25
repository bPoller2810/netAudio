using netAudio.core.Extensions;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace netAudio.core.Sources
{

    /// <summary>
    /// A Source that receives Data from a UDP Socket
    /// </summary>
    public class SocketAudioSource : IAudioSource
    {
        #region private member
        private readonly Socket _client;
        private readonly IPEndPoint _sender;

        private Thread _receiveWorker;
        private bool _working;
        #endregion

        #region ctor
        /// <summary>
        /// Creates the Audio Target that receives the data from the Socket
        /// </summary>
        /// <param name="client">The open and ready to receive Socket used to communicate</param>
        public SocketAudioSource(Socket socket, IPEndPoint sender)
        {
            if (socket is null) { throw new ArgumentNullException(nameof(socket)); }
            if (socket.SocketType != SocketType.Dgram) { throw new Exception("Can only work with Dgram SocketType"); }
            if (sender is null) { throw new ArgumentNullException(nameof(sender)); }

            _client = socket;
            _sender = sender;
        }
        #endregion

        #region buffer worker
        private void ReceiveWorker()
        {
            var buffer = new byte[_client.ReceiveBufferSize];
            var dataSender = (EndPoint)new IPEndPoint(IPAddress.Any, 0);
            while (_working)
            {
                try
                {
                    if (_client.Available == 0)
                    {//the queue seems empty, we just wait until we have data
                        Thread.Sleep(10);
                        continue;
                    }

                    var received = _client.ReceiveFrom(buffer, ref dataSender);
                    if (_sender.Equals(dataSender))//ensure only the valid bytes get used
                    {
                        AudioCaptured?.Invoke(this, buffer.SubArray(0, received));
                    }
                }
                catch (Exception ex)
                {
                    OnError?.Invoke(this, ex);
                }
            }
        }
        #endregion

        #region IAudioSource
        public event EventHandler<Exception> OnError;
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
