﻿using System.IO;
using System.Net;

namespace Tests.RecordAndPlayback;

public class HttpResponseSample
{
    public string GetResponse(string requestUri, string operationType)
    {
        var webRequest = WebRequest.Create(requestUri) as HttpWebRequest;
        if (webRequest == null)
            throw new WebException($"Could not create a HttpWebRequest for requested resource '{requestUri}'");

        webRequest.Method = operationType;

        using (var httpWebResponse = (HttpWebResponse)webRequest.GetResponse())
        {
            var stream = httpWebResponse.GetResponseStream();
            return stream != null ? new StreamReader(stream).ReadToEnd() : string.Empty;
        }
    }
}