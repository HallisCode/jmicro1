
using System;
using System.Threading.Tasks;
using Network.Core.Server.Models;
using SimpleNetFramework.Core.Server;

namespace jmicro1.Adapters
{
    public class ServerAdapter : IServer
    {
        private Network.Core.Server.IServer _server;

        public ServerAdapter(Network.Core.Server.IServer server)
        {
            _server = server;
        }

        public void SetHandler(Func<IServerRequest, Task> handler)
        {
            Func<IServerHttpRequest, Task> funcAdapter = async (IServerHttpRequest request) =>
            {
                ServerRequestAdapter serverRequestAdapter = new ServerRequestAdapter(request);

                await handler.Invoke(serverRequestAdapter);
            };

            _server.SetHandler(funcAdapter);
        }

        public async Task StartAsync()
        {
            await _server.StartAsync();
        }

        public async Task StopAsync()
        {
            await _server.StopAsync();
        }

        public void Dispose()
        {
            _server.Dispose();
        }
    }
}