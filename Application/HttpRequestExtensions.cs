using System.Net;

namespace Application;

public static class HttpRequestExtensions
{
    public static string UserAgent(this HttpRequest request)
    {
        var agentHeader = request.Headers["User-Agent"];
        return agentHeader.Count > 0 ? agentHeader[0] ?? string.Empty : string.Empty;
    }

    public static IPAddress IpAddress(this HttpRequest request)
    {
        return request.HttpContext.Connection.RemoteIpAddress ?? IPAddress.None;
    }
}