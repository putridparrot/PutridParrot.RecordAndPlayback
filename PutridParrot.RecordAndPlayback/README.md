# Record and Playback

 Based on ideas such as https://github.com/mleech/scotch. This library allows us
 to record interactions with methods and switch to replay these recorded sessions.

 The problem I had with the Scotch library was simply that it was tied too much to 
 a specific class so was not helpful in legacy code or in situations where I wanted 
 to use REST libraries or similar.

 To use, we simply wrap our method calls within  the Recorder's Invoke method and 
 passing our method call as an Expression, for example

 ```csharp
var player = new Recorder(recorder);
player.Invoke(() => calc.Add(3, 2), RecorderMode.Record);
```

In this example we're calling a Calculator's Add method in Record mode, ence the results
of the call will be stored via whhatever IRecorderStorage implementation we use. Switching
RecorderMode to Playback will then replay the method call Add. In a situation where this 
calls a webservice or the likes, this will obviously no longer make the call to the service.
Which means this is useful for testing your code (via unit test or simply in an offline mode).

This is very much in an "alpha" state and works in very specific scenarios, but I'll update 
as and when/if needed.
