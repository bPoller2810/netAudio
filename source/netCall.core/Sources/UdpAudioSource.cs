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
    public class UdpAudioSource : IAudioSource
    {
        #region private member
        private readonly UdpClient _client;
        private IPEndPoint _remote;

        private Thread _receiveWorker;
        private bool _working;
        #endregion

        #region ctor
        /// <summary>
        /// Creates the Audio Target that receives the data from the Socket
        /// </summary>
        /// <param name="client">The open and ready to receive Socket used to communicate</param>
        public UdpAudioSource(UdpClient client, IPEndPoint remote)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _remote = remote ?? throw new ArgumentNullException(nameof(remote));
        }
        #endregion

        #region buffer worker
        private void ReceiveWorker()
        {
            while (_working)
            {
                try
                {
                    if (_client.Available == 0)
                    {//the queue seems empty, we just wait until we have data
                        Thread.Sleep(1);
                        continue;
                    }

                    var received = _client.Receive(ref _remote);

                    if (received.Length > 0)
                    {
                        AudioCaptured?.Invoke(this, received);
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
