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

    public object? Playback(Invocation invocationPattern)
    {
        var match = Find(invocationPattern) ?? throw new NoRecordingExistsException();
        return match.Result;
    }

    public List<Invocation>? this[string key] => 
        _dictionary.TryGetValue(key, out var value) ? value : null;
}