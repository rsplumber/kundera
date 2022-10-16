using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Auth;

public abstract class AbstractAuthController : ControllerBase
{
    protected IPAddress IpAddress()
    {
        if (Request.Headers.ContainsKey("X-Real-IP"))
        {
            return IPAddress.Parse(Request.Headers["X-Real-IP"]);
        }

        return HttpContext.Connection.RemoteIpAddress?.MapToIPv4() ?? IPAddress.None;
    }
}