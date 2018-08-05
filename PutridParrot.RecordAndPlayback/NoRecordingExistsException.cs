using System;

namespace PutridParrot.RecordAndPlayback
{
    public sealed class NoRecordingExistsException : Exception
    {
        public NoRecordingExistsException() :
            base()
        {
        }

        public NoRecordingExistsException(string message) :
            base(message)
        {
        }
    }
}
