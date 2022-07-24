using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using PutridParrot.RecordAndPlayback;

namespace Tests.RecordAndPlayback;

[ExcludeFromCodeCoverage]
[TestFixture]
public class RecordAndReplayTests
{
    [Test]
    public void Bypass_ExpectResultAsPerTheCalledMethod()
    {
        var recorder = new InMemoryRecorderStorage();
        var calc = new CalculatorSample();

        var player = new Recorder(recorder);
        var result = player.Invoke(() => calc.Add(1, 2), RecorderMode.Bypass);

        Assert.That(result, Is.EqualTo(3));
    }

    [Test]
    public void Record_ExpectResultAsPerTheCalledMethod()
    {
        var recorder = new InMemoryRecorderStorage();
        var calc = new CalculatorSample();

        var player = new Recorder(recorder);
        var result = player.Invoke(() => calc.Add(1, 2), RecorderMode.Record);

        Assert.That(result, Is.EqualTo(3));
    }

    [Test]
    public void Play_ExpectResultAsPerTheCalledMethod()
    {
        var recorder = new InMemoryRecorderStorage();
        var calc = new CalculatorSample();

        var player = new Recorder(recorder);

        player.Invoke(() => calc.Add(1, 2), RecorderMode.Record);
        var result = player.Invoke(() => calc.Add(1, 2), RecorderMode.Playback);

        Assert.That(result, Is.EqualTo(3));
    }

    [Test]
    public void Play_WhenNotRecorded_ExpectException()
    {
        var recorder = new InMemoryRecorderStorage();
        var calc = new CalculatorSample();

        var player = new Recorder(recorder);

        player.Invoke(() => calc.Add(3, 2), RecorderMode.Record);
        Assert.Throws<NoRecordingExistsException>(() => player.Invoke(() => calc.Add(1, 2), RecorderMode.Playback));
    }


    [Test]
    public void Play_UsingNonRecordedOverloads_ExpectException()
    {
        var recorder = new InMemoryRecorderStorage();
        var calc = new CalculatorSample();

        var player = new Recorder(recorder);

        player.Invoke(() => calc.Add(1, 2), RecorderMode.Record);
        Assert.Throws<NoRecordingExistsException>(() => player.Invoke(() => calc.Add(1.0, 2.0), RecorderMode.Playback));
    }


    [Test, Ignore("Run manually to test file storage")]
    public void Test()
    {
        var recorder = new FileRecorderStorage(@"C:\Development\RecordAndReplay\recordings");
        var calc = new CalculatorSample();
        recorder.Load();

        var player = new Recorder(recorder);

        var http = new HttpResponseSample();
        //var a1 = player.Invoke(
        //    () => http.GetResponse(
        //        "www.some-rest-com, "GET"),
        //    RecordAndReplayMode.Record);

        //var a2 = player.Invoke(
        //    () => http.GetResponse(
        //        "www.someurl.com", "GET"),
        //    RecordAndReplayMode.Record);

        //var a3 = player.Invoke(
        //    () => http.GetResponse(
        //        "www.bbc.co.uk", "GET"),
        //    RecordAndReplayMode.Record);

        //recorder.Save();
        //recorder.Clear();
        //recorder.Load();

        var b = player.Invoke(
            () => http.GetResponse(
                "www.google.co.uk", "GET"),
            RecorderMode.Playback);

        //            Assert.That(a1, Is.EqualTo(b));
    }
}