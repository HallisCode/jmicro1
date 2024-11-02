using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SimpleNetFramework.Core.Server;
using ThinServer.HTTP;
using ThinServer.HTTP.Types;

namespace jmicro1.Adapters
{
    public class HttpObjectRequestAdapter : IHttpObject
    {
        private readonly IHttpRequest _httpRequest;


        public HttpMethod? Method { get; private set; }
        public string? URL { get; private set; }

        public HttpStatusCode? Code { get; private set; }
        public string? Message { get; private set; }

        public HttpProtocol? Protocol { get; private set; }

        public bool IsRequest { get; private set; }

        public ReadOnlyDictionary<string, string>? Headers { get; private set; }
        public ReadOnlyCollection<byte>? Body { get; private set; }


        public HttpObjectRequestAdapter(IHttpRequest httpRequest)
        {
            if (httpRequest == null) throw new ArgumentNullException(nameof(httpRequest));

            _httpRequest = httpRequest;

            Method = _ConvertMethod(_httpRequest.Method);
            URL = _httpRequest.Route;

            Code = null;
            Message = null;

            Protocol = _ConvertProtocol(_httpRequest.Protocol);

            IsRequest = true;

            Headers = httpRequest.Headers is null
                ? new Dictionary<string, string>().AsReadOnly()
                : httpRequest.Headers.AsReadOnly();
            Body = httpRequest.Body is null ? new byte[0].AsReadOnly() : httpRequest.Body.AsReadOnly();
        }

        private HttpMethod? _ConvertMethod(SimpleNetFramework.Core.Server.Types.HttpMethod method)
        {
            return method switch
            {
                SimpleNetFramework.Core.Server.Types.HttpMethod.Get => HttpMethod.Get,
                SimpleNetFramework.Core.Server.Types.HttpMethod.Post => HttpMethod.Post,
                SimpleNetFramework.Core.Server.Types.HttpMethod.Put => HttpMethod.Put,
                SimpleNetFramework.Core.Server.Types.HttpMethod.Delete => HttpMethod.Delete,
                _ => null,
            };
        }

        private HttpProtocol? _ConvertProtocol(string protocol)
        {
            return protocol switch
            {
                "HTTP/1.1" => HttpProtocol.Http1_1,
                "HTTP/2.0" => HttpProtocol.Http2,
                _ => null,
            };
        }
    }
}