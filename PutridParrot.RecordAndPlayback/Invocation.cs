using System;
using System.Linq;

namespace PutridParrot.RecordAndPlayback;

/// <summary>
/// Tracks the invocation details which include the
/// Assembly.Type.MethodName and the arguments
/// passed, including the types, also the result.
/// </summary>
public sealed class Invocation
{
    /// <summary>
    /// Creates an Invocation with the supplied name, arguments and result
    /// </summary>
    /// <param name="name"></param>
    /// <param name="arguments"></param>
    /// <param name="result"></param>
    public Invocation(string? name, Argument[]? arguments, object? result)
    {
        Name = name;
        Arguments = arguments;
        Result = result;
    }

    /// <summary>
    /// Gets the Name of the Invocation
    /// </summary>
    public string? Name { get; }
    /// <summary>
    /// Gets the Arguments of the Invocation
    /// </summary>
    public Argument[]? Arguments { get; }
    /// <summary>
    /// Gets the Result of the Invocation
    /// </summary>
    public object? Result { get; }

    /// <summary>
    /// Checks if the supplied Invocation is equivalent to to this Invocation
    /// </summary>
    /// <param name="invocation"></param>
    /// <returns></returns>
    public bool Equals(Invocation invocation)
    {
        var found = false;
        if (invocation.Arguments != null && Arguments != null && Arguments.Length == invocation.Arguments.Length)
        {
            found = !Arguments
                .Where((t, i) => !Object.Equals(t.Value, invocation.Arguments[i].Value))
                .Any();
        }
        return found;
    }
}