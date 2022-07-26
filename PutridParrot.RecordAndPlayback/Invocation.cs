using System;
using System.Linq;

namespace PutridParrot.RecordAndPlayback;

/// <summary>
/// Tracks the invocation details which include the
/// Assembly.Type.MethodName and the arguments
/// passed, including the types, also the result.
/// </summary>
public class Invocation
{
    public Invocation(string? name, Argument[]? arguments, object? result)
    {
        Name = name;
        Arguments = arguments;
        Result = result;
    }

    public string? Name { get; }
    public Argument[]? Arguments { get; }
    public object? Result { get; }

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