using System;
using System.Collections.Generic;
using System.Text;

namespace PutridParrot.RecordAndPlayback
{
    public interface IRecorderStorage
    {
        void Record(Invocation invocationPattern);
        object Playback(Invocation invocationPattern);
    }
}
