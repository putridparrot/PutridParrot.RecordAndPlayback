namespace PutridParrot.RecordAndPlayback
{
    /// <summary>
    /// The recorder mode stipulates whether
    /// we record, playback or bypass the 
    /// recorder
    /// </summary>
    public enum RecorderMode
    {
        // Bypass recorder/replayer
        Bypass,
        // Record mode, sends data to a recorder
        Record,
        // Playback mode, replays data from the recorder
        Playback
    }
}
