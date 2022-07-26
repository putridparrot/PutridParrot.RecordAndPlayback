namespace PutridParrot.RecordAndPlayback;

/// <summary>
/// Recorder storage should implement this interface
/// </summary>
public interface IRecorderStorage
{
    /// <summary>
    /// Record the invocation
    /// </summary>
    /// <param name="invocationPattern"></param>
    void Record(Invocation invocationPattern);
    /// <summary>
    /// Playback the invocation
    /// </summary>
    /// <param name="invocationPattern"></param>
    /// <returns></returns>
    object? Playback(Invocation invocationPattern);
}