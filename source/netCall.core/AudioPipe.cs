using netAudio.core.Targets;
using netAudio.core.Processors;
using netAudio.core.Sources;
using System;

namespace netAudio.core
{

    /// <summary>
    /// A Pipe to transmit Audio from your Source to a Target.
    /// AudioProcessors allow to modify the data
    /// </summary>
    public sealed class AudioPipe : IDisposable
    {
        #region private member
        private readonly IAudioSource _source;
        private readonly IAudioTarget _target;
        private readonly IAudioProcessor[] _processors;
        #endregion

        #region ctor
        /// <summary>
        /// Creates a AudioPipe instance
        /// </summary>
        /// <param name="source">The Source of your Audio Data</param>
        /// <param name="target">The Target(or Output) of your Audio</param>
        /// <param name="processors">optional processors to modify your data</param>
        /// <exception cref="ArgumentNullException"></exception>
        public AudioPipe(IAudioSource source, IAudioTarget target, params IAudioProcessor[] processors)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _target = target ?? throw new ArgumentNullException(nameof(target));
            _processors = processors;
        }
        #endregion

        #region public interaction
        /// <summary>
        /// Starts processing incomming Audio Data
        /// </summary>
        public void Open()
        {
            _source.AudioCaptured += HandleIncommingAudio;
        }
        /// <summary>
        /// Stops processing incomming Audio Data
        /// </summary>
        public void Close()
        {
            _source.AudioCaptured -= HandleIncommingAudio;
        }
        #endregion

        #region event handling
        private void HandleIncommingAudio(object sender, byte[] data)
        {
            foreach (var stage in _processors)
            {
                data = stage.Process(data);
            }
            _target.OutputAudioData(data);
        }
        #endregion

        #region IDisposable
        private bool _disposedValue;
        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _source.AudioCaptured -= HandleIncommingAudio;
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
