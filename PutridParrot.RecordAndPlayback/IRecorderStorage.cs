namespace PutridParrot.RecordAndPlayback;

public interface IRecorderStorage
{
    void Record(Invocation invocationPattern);
    object? Playback(Invocation invocationPattern);
}