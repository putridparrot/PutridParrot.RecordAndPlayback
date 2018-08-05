using System;
using System.Linq.Expressions;

namespace PutridParrot.RecordAndPlayback
{
    public class Recorder : IRecorder
    {
        private readonly IRecorderStorage _recorder;

        public Recorder(IRecorderStorage recorder)
        {
            _recorder = recorder;
        }

        public TResult Invoke<TResult>(Expression<Func<TResult>> expression, RecorderMode mode)
        {
            switch (mode)
            {
                case RecorderMode.Bypass:
                    return expression.Compile().Invoke();
                case RecorderMode.Record:
                    return Record(expression);
                case RecorderMode.Playback:
                    return Playback(expression);
            }

            return default(TResult);
        }

        private TResult Playback<TResult>(Expression<Func<TResult>> expression)
        {
            var result = _recorder?.Playback(CreateInvocationPattern(expression));
            return (TResult)result;
        }

        private Invocation CreateInvocationPattern<TResult>(Expression<Func<TResult>> expression, object result = null)
        {
            Argument[] arguments = null;

            var methodCallExpression = expression.Body as MethodCallExpression;
            if (methodCallExpression != null)
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

        private TResult Record<TResult>(Expression<Func<TResult>> expression)
        {
            var result = expression.Compile().Invoke();

            _recorder?.Record(CreateInvocationPattern(expression, result));

            return result;
        }

        private string GetName<TResult>(Expression<Func<TResult>> expression)
        {
            var methodCallExpression = expression.Body as MethodCallExpression;
            if (methodCallExpression != null)
            {
                var assemblyName = methodCallExpression.Method.DeclaringType.Assembly.GetName().Name;
                return $"{assemblyName}{methodCallExpression.Method.DeclaringType.Name}.{methodCallExpression.Method.Name}";
            }
            return null;
        }

        private object GetArgumentValue(Expression expression)
        {
            var constExpression = expression as ConstantExpression;
            if (constExpression != null)
            {
                return constExpression.Value;
            }
            var methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression != null)
            {
                return Expression.Lambda(methodCallExpression).Compile().DynamicInvoke();
            }

            return null;
        }
    }
}
