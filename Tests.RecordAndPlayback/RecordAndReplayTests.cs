using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;
using PutridParrot.RecordAndPlayback;

namespace Tests.RecordAndPlayback;

[ExcludeFromCodeCoverage]
[TestFixture]
public class RecordAndReplayTests
{
    [Test, Description("Bypass will simply compile and invoke the expression, no recording takes place")]
    public void Bypass_ExpectResultAsPerTheCalledMethod()
    {
        var recorder = new InMemoryRecorderStorage();
        var calc = new CalculatorSample();

        var player = new Recorder(recorder);
        var result = player.Invoke(() => calc.Add(1, 2), RecorderMode.Bypass);

        Assert.That(result, Is.EqualTo(3));
    }

    [Test, Description("Record a value but also return the value as expected")]
    public void Record_ExpectResultAsPerTheCalledMethod()
    {
        var recorder = new InMemoryRecorderStorage();
        var calc = new CalculatorSample();

        var player = new Recorder(recorder);
        var result = player.Invoke(() => calc.Add(1, 2), RecorderMode.Record);

        Assert.That(result, Is.EqualTo(3));
    }

    [Test, Description("Record a value then playback to ensure the same value is returned")]
    public void Play_ExpectResultAsPerTheCalledMethod()
    {
        var recorder = new InMemoryRecorderStorage();
        var calc = new CalculatorSample();

        var player = new Recorder(recorder);

        var result1 = player.Invoke(() => calc.Add(1, 2), RecorderMode.Record);
        var result2 = player.Invoke(() => calc.Add(1, 2), RecorderMode.Playback);

        Assert.That(calc.CallCount, Is.EqualTo(1));
        Assert.That(result2, Is.EqualTo(result1));
    }

    [Test, Description("Playing back an expression that was not record, results in an exception")]
    public void Play_WhenNotRecorded_ExpectException()
    {
        var recorder = new InMemoryRecorderStorage();
        var calc = new CalculatorSample();

        var player = new Recorder(recorder);

        player.Invoke(() => calc.Add(3, 2), RecorderMode.Record);
        Assert.Throws<NoRecordingExistsException>(() => player.Invoke(() => calc.Add(1, 2), RecorderMode.Playback));
    }

    [Test, Description("The expression has the same arguments but of different types, so playback should fail as the recorded expression is not the same")]
    public void Play_UsingNonRecordedOverloads_ExpectException()
    {
        var recorder = new InMemoryRecorderStorage();
        var calc = new CalculatorSample();

        var player = new Recorder(recorder);

        player.Invoke(() => calc.Add(1, 2), RecorderMode.Record);
        Assert.Throws<NoRecordingExistsException>(() => player.Invoke(() => calc.Add(1.0, 2.0), RecorderMode.Playback));
    }

    [Test, Description("Record and playback using async methods")]
    public async Task InvokeAsync_RecordAndPlayback_EnsureResultIsCorrect()
    {
        var recorder = new InMemoryRecorderStorage();
        var calc = new CalculatorSample();

        var player = new Recorder(recorder);

        var result1 = await player.InvokeAsync(() => calc.AddAsync(1, 2), RecorderMode.Record);
        var result2 = await player.InvokeAsync(() => calc.AddAsync(1, 2), RecorderMode.Playback);

        Assert.That(result2, Is.EqualTo(result1));
        Assert.That(1, Is.EqualTo(calc.CallCount));
    }

    // simulate getting arguments from a method
    private int Arg(int value) => value;

    [Test, Description("Record and playback async methods where the parameters are supplied by methods")]
    public async Task InvokeAsync_RecordAndPlayback_UsingMethodsForArgs_EnsureResultIsCorrect()
    {
        var recorder = new InMemoryRecorderStorage();
        var calc = new CalculatorSample();

        var player = new Recorder(recorder);

        var result1 = await player.InvokeAsync(() => calc.AddAsync(Arg(1), Arg(2)), RecorderMode.Record);
        var result2 = await player.InvokeAsync(() => calc.AddAsync(Arg(1), Arg(2)), RecorderMode.Playback);

        Assert.That(result2, Is.EqualTo(result1));
        Assert.That(1, Is.EqualTo(calc.CallCount));
    }

    [Test, Description("Record but playback a different expression, expect and exception")]
    public async Task InvokeAsync_RecordAndPlayback_UsingMethodsForArgs_WithDifferentValues_ExpectException()
    {
        var recorder = new InMemoryRecorderStorage();
        var calc = new CalculatorSample();

        var player = new Recorder(recorder);

        await player.InvokeAsync(() => calc.AddAsync(Arg(1), Arg(2)), RecorderMode.Record);
        Assert.ThrowsAsync<NoRecordingExistsException>(async () => await player.InvokeAsync(() => calc.AddAsync(Arg(1), Arg(3)), RecorderMode.Playback));
    }


    [Test, Explicit("Run manually to test file storage")]
    public void Test()
    {
        var filestore = $"{Path.GetDirectoryName(Assembly.GetAssembly(typeof(Recorder)).Location)}\\recordings";
        var recorder = new FileRecorderStorage(filestore);
        var calc = new CalculatorSample();
        recorder.Load();

        var player = new Recorder(recorder);

        var http = new HttpResponseSample();

        //recorder.Save();
        //recorder.Clear();
        //recorder.Load();

        var b = player.Invoke(
            () => http.GetResponse(
                "http://www.google.co.uk", HttpMethod.Get),
            RecorderMode.Playback);

        //recorder.Save();

        //            Assert.That(a1, Is.EqualTo(b));
    }

    [Test, Explicit("Run manually to test file storage")]
    public async Task TestAsync()
    {
        var filestore = $"{Path.GetDirectoryName(Assembly.GetAssembly(typeof(Recorder)).Location)}\\recordings";
        var recorder = new FileRecorderStorage(filestore, "async-recordings.json");
        var calc = new CalculatorSample();
        recorder.Load();

        var player = new Recorder(recorder);

        var http = new HttpResponseSample();

        var b = await player.InvokeAsync(
            () => http.GetResponseAsync(
                "http://www.google.co.uk", HttpMethod.Get),
            RecorderMode.Playback);

        //recorder.Save();
    }
}