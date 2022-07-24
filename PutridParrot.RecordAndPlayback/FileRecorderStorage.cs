using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace PutridParrot.RecordAndPlayback;

/// <summary>
/// An IRecorder which persists data to file.
/// </summary>
public class FileRecorderStorage : IRecorderStorage
{
    private readonly string _rootFolder;
    private Dictionary<string, List<Invocation>>? _dictionary;

    public FileRecorderStorage(string rootFolder)
    {
        _rootFolder = rootFolder;
        _dictionary = new Dictionary<string, List<Invocation>>();
    }

    public void Load()
    {
        // simple version stores everything in a single file
        var dataFile = $"{_rootFolder}{Path.DirectorySeparatorChar}recording.json";
        if (File.Exists(dataFile))
        {
            var json = File.ReadAllText(dataFile);
            if (!String.IsNullOrEmpty(json))
            {
                _dictionary = JsonConvert.DeserializeObject<Dictionary<string, List<Invocation>>>(json);
            }
        }
    }

    public void Save()
    {
        var dataFile = $"{_rootFolder}/recording.json";
        File.WriteAllText(dataFile, JsonConvert.SerializeObject(_dictionary));
    }

    private Invocation? Find(Invocation invocationPattern)
    {
        if (_dictionary != null && invocationPattern.Name != null)
        {
            if (_dictionary.TryGetValue(invocationPattern.Name, out var invocationPatterns))
            {
                return invocationPatterns.FirstOrDefault(
                    i => i.Equals(invocationPattern));
            }
        }

        return null;
    }

    public void Clear()
    {
        _dictionary?.Clear();
    }

    public void Record(Invocation invocationPattern)
    {
        if (_dictionary != null && !String.IsNullOrEmpty(invocationPattern.Name))
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
        var match = Find(invocationPattern);
        if (match == null)
        {
            throw new NoRecordingExistsException();
        }

        return match.Result;
    }

    public List<Invocation>? this[string key] => 
        _dictionary != null && _dictionary.TryGetValue(key, out var value) ? value : null;
}