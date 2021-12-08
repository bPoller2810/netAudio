using MvvMHelpers.core;
using netAudio.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace netAudio.sample.wpf
{
    public class MainViewModel : BaseViewModel
    {
        private AudioPipe _recordPipe;

        public ICommand StartRecordCommand { get;init;}
        public ICommand StopRecordCommand { get;init;}
        public ICommand StartPlaybackCommand { get;init;}
        public ICommand StopPlaybackCommand { get;init;}


        public MainViewModel()
        {
            StartRecordCommand = new AsyncCommand(HandleStartRecord);
            StopRecordCommand = new AsyncCommand(HandleStopRecord);
            StartPlaybackCommand = new AsyncCommand(HandleStartPlayback);
            StopPlaybackCommand = new AsyncCommand(HandleStopPlayback);
        }

        private Task HandleStopPlayback(object arg)
        {
            throw new NotImplementedException();
        }

        private Task HandleStartPlayback(object arg)
        {
            throw new NotImplementedException();
        }

        private Task HandleStopRecord(object arg)
        {
            throw new NotImplementedException();
        }

        private Task HandleStartRecord(object arg)
        {
            throw new NotImplementedException();
        }
    }
}
