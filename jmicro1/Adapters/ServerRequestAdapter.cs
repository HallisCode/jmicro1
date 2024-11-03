using System;
using Network.Core.Server.Models;
using SimpleNetFramework.Core.Server;

namespace jmicro1.Adapters
{
    public class ServerRequestAdapter : IServerRequest
    {
        private readonly IServerHttpRequest _serverHttpRequest;

        public IHttpRequest Request { get; private set; }

        public IHttpResponse? Response
        {
            get
            {
                if (_serverHttpRequest.Response is not null)
                {
                    return new HttpResponseAdapter(_serverHttpRequest.Response);
                }

                return null;
            }
            set
            {
                _serverHttpRequest.Response = new HttpObjectResponseAdapter(value);
            }
        }

        public bool isResponseSet => _serverHttpRequest.IsResponseSet;


        public ServerRequestAdapter(IServerHttpRequest serverHttpRequest)
        {
            if (serverHttpRequest == null) throw new ArgumentNullException(nameof(serverHttpRequest));

            _serverHttpRequest = serverHttpRequest;

            Request = new HttpRequestAdapter(serverHttpRequest.Request);
        }
    }
}