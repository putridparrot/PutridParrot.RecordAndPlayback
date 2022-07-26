using System;

namespace PutridParrot.RecordAndPlayback;

/// <summary>
/// Thrown where no recording exists for an invocation
/// </summary>
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