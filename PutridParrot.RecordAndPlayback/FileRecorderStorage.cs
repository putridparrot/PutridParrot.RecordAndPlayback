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
    private const string DefaultFilename = "recording.json";

    private readonly string _rootFolder;
    private readonly string _fileName;
    private Dictionary<string, List<Invocation>>? _dictionary;

    /// <summary>
    /// Creates and instance of FileRecorderStorage
    /// </summary>
    /// <param name="rootFolder"></param>
    /// <param name="fileName"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public FileRecorderStorage(string? rootFolder, string? fileName = null)
    {
        _rootFolder = rootFolder ?? throw new ArgumentNullException(nameof(rootFolder));
        _fileName = fileName ?? DefaultFilename;
        _dictionary = new Dictionary<string, List<Invocation>>();
    }

    private string RecordingFile => $"{_rootFolder}{Path.DirectorySeparatorChar}{_fileName}";

    /// <summary>
    /// Load the cache from file
    /// </summary>
    public void Load()
    {
        // simple version stores everything in a single file
        if (File.Exists(RecordingFile))
        {
            var json = File.ReadAllText(RecordingFile);
            if (!String.IsNullOrEmpty(json))
            {
                _dictionary = JsonConvert.DeserializeObject<Dictionary<string, List<Invocation>>>(json);
            }
        }
    }

    /// <summary>
    /// Save the cache to file
    /// </summary>
    public void Save()
    {
        if (String.IsNullOrEmpty(_rootFolder))
        {
            return;
        }

        if (!Directory.Exists(_rootFolder))
        {
            Directory.CreateDirectory(_rootFolder);
        }

        File.WriteAllText(RecordingFile, JsonConvert.SerializeObject(_dictionary));
    }

    private Invocation? Find(Invocation invocationPattern)
    {
        if (_dictionary != null && !String.IsNullOrEmpty(invocationPattern.Name))
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
    /// Clear the cache of Invocations
    /// </summary>
    public void Clear()
    {
        _dictionary?.Clear();
    }

    /// <summary>
    /// Record the Invocation
    /// </summary>
    /// <param name="invocationPattern"></param>
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
    public List<Invocation>? this[string key] => 
        _dictionary != null && _dictionary.TryGetValue(key, out var value) ? value : null;
}