using System;
using System.Collections.Generic;
using System.Linq;
using SimpleNetFramework.Core.Server;
using SimpleNetFramework.Core.Server.Types;
using ThinServer.HTTP;

namespace jmicro1.Adapters
{
    public class HttpResponseAdapter : IHttpResponse
    {
        private readonly IHttpObject _httpObject;

        public string Protocol { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public Dictionary<string, string> Headers { get; set; }
        public byte[] Body { get; set; }

        public HttpResponseAdapter(IHttpObject httpObject)
        {
            if (httpObject == null) throw new ArgumentNullException(nameof(httpObject));
            if (httpObject.IsRequest) throw new InvalidOperationException("Cannot use a request object as a response.");

            _httpObject = httpObject;

            Protocol = _httpObject.Protocol!.ToString();
            StatusCode = _httpObject.Code!.Value.Code;
            Message = _httpObject.Message!;
            Headers = _httpObject.Headers is null
                ? new Dictionary<string, string>()
                : _httpObject.Headers.ToDictionary();
            Body = _httpObject.Body is null ? new byte[0] : _httpObject.Body.ToArray();
        }
    }
}