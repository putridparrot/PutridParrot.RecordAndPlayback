using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Tests.RecordAndPlayback;

[ExcludeFromCodeCoverage]
public class CalculatorSample
{
    public int CallCount { get; set; } = 0;

    public int Add(int a, int b)
    {
        CallCount++;
        return a + b;
    }

    public Task<int> AddAsync(int a, int b)
    {
        CallCount++;
        return Task.FromResult(a + b);
    }

    public double Add(double a, double b)
    {
        CallCount++;
        return a + b;
    }

    public int Subtract(int a, int b)
    {
        CallCount++;
        return a - b;
    }
}