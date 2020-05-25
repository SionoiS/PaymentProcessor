using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NetTools;
using System;
using System.Threading.Tasks;

namespace PaymentProcessor
{
    public class AdminSafeListMiddleware
    {
        readonly RequestDelegate next;
        readonly ILogger<AdminSafeListMiddleware> logger;
        readonly IPAddressRange[] whitelist;

        public AdminSafeListMiddleware(RequestDelegate next, ILogger<AdminSafeListMiddleware> logger)
        {
            var envar = Environment.GetEnvironmentVariable("IP_WHITE_LIST");
            var ips = envar.Split(';');

            whitelist = new IPAddressRange[ips.Length];
            for (int i = 0; i < ips.Length; i++)
            {
                whitelist[i] = IPAddressRange.Parse(ips[i]);
            }

            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var remoteIp = context.Connection.RemoteIpAddress;
            logger.LogDebug("Request from Remote IP address: {RemoteIp}", remoteIp);

            var badIp = true;
            foreach (var addressRange in whitelist)
            {
                if (addressRange.Contains(remoteIp))
                {
                    badIp = false;
                    break;
                }
            }

            if (badIp)
            {
                logger.LogWarning("Forbidden Request from Remote IP address: {RemoteIp}", remoteIp);
                context.Response.StatusCode = 401;
                return;
            }
            
            await next(context);
        }
    }
}
