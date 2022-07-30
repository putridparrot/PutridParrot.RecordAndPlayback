using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PutridParrot.RecordAndPlayback;

/// <summary>
/// A recorder implements the IRecorder interface
/// </summary>
public interface IRecorder
{
    /// <summary>
    /// Invokes an expression
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="expression"></param>
    /// <param name="mode"></param>
    /// <returns></returns>
    TResult? Invoke<TResult>(Expression<Func<TResult?>> expression, RecorderMode mode);

    /// <summary>
    /// Invokes an expression
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="expression"></param>
    /// <param name="mode"></param>
    /// <returns></returns>
    Task<TResult?> InvokeAsync<TResult>(Expression<Func<Task<TResult?>>> expression, RecorderMode mode);
}