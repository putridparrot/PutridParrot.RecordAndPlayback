using System;
using System.Collections.Generic;
using System.Text;

namespace PutridParrot.RecordAndPlayback
{
    public interface IRecorder
    {
        void Record(Invocation invocationPattern);
        object Playback(Invocation invocationPattern);
    }
}
