using System;
using System.Collections.Generic;
using System.Linq;
using SimpleNetFramework.Core.Server;
using SimpleNetFramework.Core.Server.Types;
using ThinServer.HTTP;

namespace jmicro1.Adapters
{
    public class HttpRequestAdapter : IHttpRequest
    {
        private readonly IHttpObject _httpObject;


        public HttpMethod Method { get; private set; }
        public string Route { get; private set; }
        public string Protocol { get; private set; }

        public Dictionary<string, string> Headers { get; private set; }
        public byte[] Body { get; private set; }

        public HttpRequestAdapter(IHttpObject httpObject)
        {
            if (httpObject == null) throw new ArgumentNullException(nameof(httpObject));
            if (!httpObject.IsRequest)
                throw new InvalidOperationException("Cannot use a response object as a request.");

            _httpObject = httpObject;

            Method = _ConvertMethod(_httpObject.Method);
            Route = _httpObject.URL ?? string.Empty;
            Protocol = _httpObject.Protocol?.ToString() ?? string.Empty;

            Headers = _httpObject.Headers is null
                ? new Dictionary<string, string>()
                : _httpObject.Headers.ToDictionary();
            Body = _httpObject.Body is null ? new byte[0] : _httpObject.Body.ToArray();
        }

        private HttpMethod _ConvertMethod(ThinServer.HTTP.Types.HttpMethod? method)
        {
            return method switch
            {
                ThinServer.HTTP.Types.HttpMethod.Get => HttpMethod.Get,
                ThinServer.HTTP.Types.HttpMethod.Post => HttpMethod.Post,
                ThinServer.HTTP.Types.HttpMethod.Put => HttpMethod.Put,
                ThinServer.HTTP.Types.HttpMethod.Delete => HttpMethod.Delete,
                _ => HttpMethod.Get
            };
        }
    }
}