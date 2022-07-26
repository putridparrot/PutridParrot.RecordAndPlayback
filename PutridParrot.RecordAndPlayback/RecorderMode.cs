namespace PutridParrot.RecordAndPlayback;

/// <summary>
/// The recorder mode stipulates whether
/// we record, playback or bypass the 
/// recorder
/// </summary>
public enum RecorderMode
{
    /// <summary>
    /// Bypass recorder/player
    /// </summary>
    Bypass,
    /// <summary>
    /// Record mode, sends data to a recorder
    /// </summary>
    Record,
    /// <summary>
    /// Playback mode, replays data from the recorder
    /// </summary>
    Playback
}