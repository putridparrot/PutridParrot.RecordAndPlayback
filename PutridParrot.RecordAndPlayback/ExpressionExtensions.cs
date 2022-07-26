using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace PutridParrot.RecordAndPlayback;

internal static class ExpressionExtensions
{
    /// <summary>
    /// Gets the method name from an Expression if it's
    /// a Expression&lt;Action&gt;
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static string GetMethodName(this Expression expression)
    {
        return expression is Expression<Action> methodCallExpression
            ? methodCallExpression.GetMethodName()
            : String.Empty;
    }

    /// <summary>
    /// Gets the method name from an Expression&lt;Action&gt;
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static string GetMethodName(this Expression<Action> expression)
    {
        return expression.Body is MethodCallExpression body ? body.Method.Name : String.Empty;
    }

    /// <summary>
    /// Gets the method name from an Expression&lt;Func&lt;T&gt;&gt;
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static string GetMethodName<T>(this Expression<Func<T>> expression)
    {
        return expression.Body is MethodCallExpression body ? body.Method.Name : String.Empty;
    }

    public static IReadOnlyCollection<Expression> GetArguments(this Expression<Action> expression)
    {
        return expression.Body is MethodCallExpression body ? body.GetArguments() : Array.Empty<Expression>();
    }

    public static IReadOnlyCollection<Expression> GetArguments<T>(this Expression<Func<T>> expression)
    {
        return expression.Body is MethodCallExpression body ? body.GetArguments() : Array.Empty<Expression>();
    }

    public static IReadOnlyCollection<Expression> GetArguments(this MethodCallExpression expression)
    {
        var args = new List<Expression>();
        foreach (var argument in expression.Arguments)
        {
            if (argument is MethodCallExpression methodCallExpression && methodCallExpression.Arguments.Count > 0)
            {
                args.AddRange(methodCallExpression.GetArguments());
            }
            else
            {
                args.Add(argument);
            }
        }
        return new ReadOnlyCollection<Expression>(args);
    }
}