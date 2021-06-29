using netAudio.core.Outputs;
using netAudio.core.Processors;
using netAudio.core.Sources;
using System;

namespace netAudio.core
{
    public class AudioPipe
    {
        #region private member
        private readonly IAudioSource _source;
        private readonly IAudioTarget _target;
        private readonly IAudioProcessor[] _processors;
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
        public void Open()
        {
            _source.AudioCaptured += HandleIncommingAudio;
        }
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

    }
}
