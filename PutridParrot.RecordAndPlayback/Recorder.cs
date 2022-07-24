using System;
using System.Linq.Expressions;

namespace PutridParrot.RecordAndPlayback;

public class Recorder : IRecorder
{
    private readonly IRecorderStorage? _recorder;

    public Recorder(IRecorderStorage? recorder)
    {
        _recorder = recorder;
    }

    public TResult? Invoke<TResult>(Expression<Func<TResult?>> expression, RecorderMode mode)
    {
        return mode switch
        {
            RecorderMode.Bypass => expression.Compile().Invoke(),
            RecorderMode.Record => Record(expression),
            RecorderMode.Playback => Playback(expression),
            _ => default
        };
    }

    private TResult? Playback<TResult>(Expression<Func<TResult>> expression)
    {
        var result = _recorder?.Playback(CreateInvocationPattern(expression));
        return result != null ? (TResult)result : default;
    }

    private Invocation CreateInvocationPattern<TResult>(Expression<Func<TResult>> expression, object? result = null)
    {
        Argument[]? arguments = null;

        if (expression.Body is MethodCallExpression methodCallExpression)
        {
            var numberOfArguments = methodCallExpression.Arguments.Count;
            arguments = new Argument[numberOfArguments];

            for (var i = 0; i < methodCallExpression.Arguments.Count; i++)
            {
                var argument = methodCallExpression.Arguments[i].Type;
                arguments[i] = new Argument(argument, GetArgumentValue(methodCallExpression.Arguments[i]));
            }
        }

        return new Invocation(GetName(expression), arguments, result);
    }

    private TResult? Record<TResult>(Expression<Func<TResult?>> expression)
    {
        var result = expression.Compile().Invoke();

        _recorder?.Record(CreateInvocationPattern(expression, result));

        return result;
    }

    private static string? GetName<TResult>(Expression<Func<TResult>> expression)
    {
        if (expression.Body is MethodCallExpression methodCallExpression)
        {
            var assemblyName = methodCallExpression.Method.DeclaringType?.Assembly.GetName().Name;
            return $"{assemblyName}{methodCallExpression.Method.DeclaringType?.Name}.{methodCallExpression.Method.Name}";
        }
        return null;
    }

    private static object? GetArgumentValue(Expression expression)
    {
        if (expression is ConstantExpression constExpression)
        {
            return constExpression.Value;
        }

        if (expression is MethodCallExpression methodCallExpression)
        {
            return Expression.Lambda(methodCallExpression).Compile().DynamicInvoke();
        }

        return null;
    }
}