using netCall.core.Outputs;
using netCall.core.Processors;
using netCall.core.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace netCall.core
{
    public class AudioPipe : IDisposable
    {
        #region private member
        private readonly IAudioSource _source;
        private readonly IAudioTarget _target;
        private readonly IAudioProcessor[] _processors;

        private bool _isDisposed;
        #endregion

        #region ctor
        public AudioPipe(IAudioSource source, IAudioTarget target, params IAudioProcessor[] processors)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _target = target ?? throw new ArgumentNullException(nameof(target));
            _processors = processors;
        }
        #endregion

        #region public interaction
        public void Activate()
        {
            if(_isDisposed)
            {
                throw new ObjectDisposedException(nameof(AudioPipe));
            }
            _source.AudioCaptured += HandleIncommingAudio;
            _source.Activate();
        }
        public void Deactivate()
        {
            _source.Deactivate();
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
        public void Dispose()
        {
            if(_isDisposed)
            {
                return;
            }
            _isDisposed = true;
            _source.Dispose();
            _target.Dispose();
            foreach (var processor in _processors)
            {
                processor.Dispose();
            }
        }
        #endregion

    }
}
