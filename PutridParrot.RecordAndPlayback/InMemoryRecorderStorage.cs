using System;
using System.Collections.Generic;
using System.Linq;

namespace PutridParrot.RecordAndPlayback;

/// <summary>
/// Simple, in memory implementation of an IRecorder.
/// All data is lost when the instance is disposed.
/// </summary>
public class InMemoryRecorderStorage : IRecorderStorage
{
    private readonly Dictionary<string, List<Invocation>> _dictionary;

    /// <summary>
    /// Creates an instance of an InMemoryRecorderStorage
    /// </summary>
    public InMemoryRecorderStorage()
    {
        _dictionary = new Dictionary<string, List<Invocation>>();
    }

    private Invocation? Find(Invocation invocationPattern)
    {
        if (!String.IsNullOrEmpty(invocationPattern.Name))
        {
            if (_dictionary.TryGetValue(invocationPattern.Name, out var invocationPatterns))
            {
                return invocationPatterns.FirstOrDefault(
                    i => i.Equals(invocationPattern));
            }
        }

        return null;
    }

    /// <summary>
    /// Record the Invocation
    /// </summary>
    /// <param name="invocationPattern"></param>
    public void Record(Invocation invocationPattern)
    {
        if (!String.IsNullOrEmpty(invocationPattern.Name))
        {
            if (_dictionary.TryGetValue(invocationPattern.Name, out var invocationPatterns))
            {
                var match = invocationPatterns.FirstOrDefault(
                    i => i.Equals(invocationPattern.Arguments));

                if (match == null)
                {
                    invocationPatterns.Add(invocationPattern);
                }
            }
            else
            {
                _dictionary.Add(invocationPattern.Name, new List<Invocation>(new[] { invocationPattern }));
            }
        }
    }

    /// <summary>
    /// Playback the supplied Invocation pattern
    /// </summary>
    /// <param name="invocationPattern"></param>
    /// <returns></returns>
    /// <exception cref="NoRecordingExistsException"></exception>
    public object? Playback(Invocation invocationPattern)
    {
        var match = Find(invocationPattern) ?? throw new NoRecordingExistsException();
        return match.Result;
    }

    /// <summary>
    /// Gets the Invocation list for the supplied key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public IList<Invocation>? this[string key] => 
        _dictionary.TryGetValue(key, out var value) ? value : null;
}