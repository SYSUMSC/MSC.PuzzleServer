﻿using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MSC.Server.Middlewares
{
    public class ProxyMiddleware
    {
        private readonly RequestDelegate _next;

        public ProxyMiddleware(RequestDelegate next) => _next = next;

        public Task Invoke(HttpContext context)
        {
            var headers = context.Request.Headers;
            if (headers.ContainsKey("X-Forwarded-For"))
            {
                var ipAddresses = headers["X-Forwarded-For"].ToString().Split(',', StringSplitOptions.RemoveEmptyEntries);
                context.Connection.RemoteIpAddress = IPAddress.Parse(ipAddresses.FirstOrDefault());
            }
            return _next(context);
        }
    }
}