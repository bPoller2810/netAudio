using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace netAudio.core.Targets
{
    /// <summary>
    /// A Target to Send data through UDP Sockets into the network
    /// </summary>
    public class UdpAudioTarget : IAudioTarget
    {
        #region private member
        private readonly UdpClient _client;
        private readonly ConcurrentQueue<byte[]> _workerQueue;
        private readonly IPEndPoint _remote;

        private Thread _bufferWorker;
        private bool _working;
        #endregion

        #region ctor
        /// <summary>
        /// Creates the Audio Target that sends the data into the Socket
        /// SocketType must be set to SocketType.Dgram
        /// </summary>
        /// <param name="socket">The soocket used to send data</param>
        /// <param name="receiver">The receiver the data gets sent to</param>
        public UdpAudioTarget(UdpClient client, IPEndPoint remote)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _remote = remote ?? throw new ArgumentNullException(nameof(remote));

            _workerQueue = new();
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
                        Thread.Sleep(1);
                        continue;
                    }


                    _client.Send(data, data.Length, _remote);
                }
                catch (Exception ex)
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

    }
}
