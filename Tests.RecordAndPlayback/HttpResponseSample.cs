using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tests.RecordAndPlayback;

[ExcludeFromCodeCoverage]
public class HttpResponseSample
{
    public string GetResponse(string requestUri, HttpMethod operationType)
    {
        var httpRequest = new HttpClient();
        var httpRequestMessage = new HttpRequestMessage(operationType, requestUri);

        using var httpWebResponse = httpRequest.Send(httpRequestMessage);
        var stream = httpWebResponse.Content.ReadAsStream();
        return new StreamReader(stream).ReadToEnd();
    }

    public async Task<string> GetResponseAsync(string requestUri, HttpMethod operationType)
    {
        var httpRequest = new HttpClient();
        var httpRequestMessage = new HttpRequestMessage(operationType, requestUri);

        using var httpWebResponse = await httpRequest.SendAsync(httpRequestMessage);
        var stream = await httpWebResponse.Content.ReadAsStreamAsync();
        return await new StreamReader(stream).ReadToEndAsync();
    }

}