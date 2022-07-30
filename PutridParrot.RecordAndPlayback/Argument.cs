using System;

namespace PutridParrot.RecordAndPlayback;

/// <summary>
/// The Argument class
/// </summary>
public sealed class Argument
{
    /// <summary>
    /// Creates an instance of an Argument
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    public Argument(Type? type, object? value)
    {
        Type = type;
        Value = value;
    }

    /// <summary>
    /// Gets the argument type
    /// </summary>
    public Type? Type { get; }
    /// <summary>
    /// Gets the argument value
    /// </summary>
    public object? Value { get; }
}