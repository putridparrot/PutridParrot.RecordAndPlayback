using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using NUnit.Framework;
using PutridParrot.RecordAndPlayback;

namespace Tests.RecordAndPlayback;

[ExcludeFromCodeCoverage]
[TestFixture]
internal class ExpressionExtensionsTests
{
    private void MethodNoArgsNoReturn()
    {
    }

    private void MethodTwoArgsNoReturn(int a, int b)
    {
    }

    private int MethodTwoArgsWithReturn(int a, int b)
    {
        return a + b;
    }

    [Test]
    public void GetMethodName_Expression1()
    {
        Expression expression = () => MethodNoArgsNoReturn();
        Assert.That(expression.GetMethodName(), Is.EqualTo("MethodNoArgsNoReturn"));
    }

    [Test]
    public void GetMethodName_Expression2()
    {
        Expression expression = () => MethodNoArgsNoReturn();
        Assert.That(expression.GetMethodName(), Is.EqualTo("MethodNoArgsNoReturn"));
    }

    [Test]
    public void GetMethodName_Expression3()
    {
        Expression<Func<int>> expression = () => MethodTwoArgsWithReturn(1, 2);
        Assert.That(expression.GetMethodName(), Is.EqualTo("MethodTwoArgsWithReturn"));
    }

    [Test]
    public void GetMethodName_ExpressionAction()
    {
        Expression<Action> expression = () => MethodTwoArgsNoReturn(1, 2);
        Assert.That(expression.GetMethodName(), Is.EqualTo("MethodTwoArgsNoReturn"));
    }

    [Test]
    public void GetArguments_ExpressionTwoNoArgs()
    {
        Expression<Action> expression = () => MethodNoArgsNoReturn();
        Assert.That(expression.GetArguments().Count, Is.EqualTo(0));
    }

    [Test]
    public void GetArguments_ExpressionTwoArgs()
    {
        Expression<Func<int>> expression = () => MethodTwoArgsWithReturn(1, 2);
        Assert.That(expression.GetArguments().Count, Is.EqualTo(2));
    }
}