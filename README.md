# Record and Playback

[![Build status](https://ci.appveyor.com/api/projects/status/c1mx2s3gxgqwld4i?svg=true)](https://ci.appveyor.com/project/putridparrot/putridparrot-recordandplayback)

Based on ideas such as https://github.com/mleech/scotch. This library allows us
to record interactions with methods and switch to replay these recorded sessions.

_**Why not just use Scotch?**_

_The problem I had with the Scotch library was simply that it was tied too much to 
a specific class so was not quite so helpful in legacy code or in situations where I wanted 
to use REST libraries or similar._

## Overview

The original andprimary requirement for this application comes from situations where I'm working on the client code
to an application and either the server may be offline often or the environment flaky or other situations
where I ultimately want more control over the supply of data to my application. 

The library is not limited to calling services, it's aimed at being flexible enough to allows us to just replace
long running code. For example maybe we have to do a lot of calculations which can be slow, instead we can 
record those calculations and just playback when needed.


## Using Record and Playback

To use, we need to create a storage mechanism which might be in memory (in which case obviously the data is lost 
when the application stops) or file based (although the developer can write their own IRecorderStorage so you 
could write to alternate storage mechanisms).

We create the _Recorder_ like this

 ```csharp
var recorder = new Recorder(new FileRecorderStorage("c:\\temp\\recordings"));
```

Now simply wrap our method calls within  the Recorder's Invoke method and 
passing our method call as an Expression, for example

```csharp
var result = recorder.Invoke(() => calc.Add(3, 2), RecorderMode.Record);
```

In this example we're calling a Calculator's Add method in _Record_ mode, hence the results
of the call will be stored via whatever IRecorderStorage implementation we use. Switching
_RecorderMode_ to _Playback_ will then playback the method call result without actually calling the 
method. Not very useful for an Add method, but if the method was carrying out Fourier analysis 
on wave form, then it might be more useful, allowing us to simply return a known result or set 
or results.

In a situation where this calls a webservice or the likes, this will obviously no longer make the 
call to the service once the _RecorderModel_ is set to _Playback_. Which means this is useful for testing 
your code (via unit tests or simply in an offline mode).

Here's an example of this using async methods

 ```csharp
var response = recorder.Invoke(
    () => http.GetResponse("http://mydataserver/getdata", HttpMethod.Get),
        RecorderMode.Playback);
```


**This is very much in an "alpha" state and works in very specific scenarios, but I'll update 
as and when/if needed.**
