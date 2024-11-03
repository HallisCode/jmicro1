using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using Network.Core.HTTP;
using Network.Core.HTTP.Types;
using SimpleNetFramework.Core.Server;

namespace jmicro1.Adapters
{
    public class HttpObjectResponseAdapter : IHttpObject
    {
        private readonly IHttpResponse _httpResponse;


        public HttpMethod? Method { get; private set; }
        public string? Message { get; private set; }
        public string? URL { get; private set; }
        public HttpStatusCode? Code { get; private set; }

        public HttpProtocol? Protocol { get; private set; }

        public bool IsRequest { get; private set; }

        public ReadOnlyDictionary<string, string>? Headers { get; private set; }
        public ReadOnlyCollection<byte>? Body { get; private set; }


        public HttpObjectResponseAdapter(IHttpResponse httpResponse)
        {
            if (httpResponse == null) throw new ArgumentNullException(nameof(httpResponse));

            _httpResponse = httpResponse;

            Method = null;
            Message = _httpResponse.Message;
            URL = null;
            Code = (HttpStatusCode)_httpResponse.StatusCode;

            Protocol = _ConvertProtocol(_httpResponse.Protocol);

            IsRequest = false;

            Headers = _httpResponse.Headers is null
                ? new Dictionary<string, string>().AsReadOnly()
                : _httpResponse.Headers.AsReadOnly();
            Body = _httpResponse.Body is null ? new byte[0].AsReadOnly() : _httpResponse.Body.AsReadOnly();
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