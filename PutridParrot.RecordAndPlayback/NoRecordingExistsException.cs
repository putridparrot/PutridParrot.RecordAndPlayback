using System;

namespace PutridParrot.RecordAndPlayback;

/// <summary>
/// Thrown where no recording exists for an invocation
/// </summary>
public sealed class NoRecordingExistsException : Exception
{
    /// <summary>
    /// Creates a default NoRecordingExistsException
    /// </summary>
    public NoRecordingExistsException() :
        base()
    {
    }

    /// <summary>
    /// Creates a NoRecordingExistsException with the supplied message
    /// </summary>
    /// <param name="message"></param>
    public NoRecordingExistsException(string message) :
        base(message)
    {
    }
}