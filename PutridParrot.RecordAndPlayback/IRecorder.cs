using System;
using System.Linq.Expressions;

namespace PutridParrot.RecordAndPlayback;

/// <summary>
/// A recorder implements the IRecorder interface
/// </summary>
public interface IRecorder
{
    TResult? Invoke<TResult>(Expression<Func<TResult?>> expression, RecorderMode mode);
}