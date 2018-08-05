using System;
using System.Linq.Expressions;

namespace PutridParrot.RecordAndPlayback
{

    public interface IRecorder
    {
        TResult Invoke<TResult>(Expression<Func<TResult>> expression, RecorderMode mode);
    }
}
