using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;

namespace netAudio.core.Targets
{
    /// <summary>
    /// A Target to Send data through Sockets into the network
    /// </summary>
    public class SocketAudioTarget : IAudioTarget
    {
        #region private member
        private readonly Socket _client;
        private readonly ConcurrentQueue<byte[]> _workerQueue;

        private Thread _bufferWorker;
        private bool _working;
        #endregion

        #region ctor
        /// <summary>
        /// Creates the Audio Target that sends the data into the Socket
        /// </summary>
        /// <param name="client">The open and ready to send Socket used to communicate</param>
        public SocketAudioTarget(Socket client)
        {
            _client = client;
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
                        Thread.Sleep(10);
                        continue;
                    }

                    _client.Send(data);
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
