using System;
using System.Threading.Tasks;
using SimpleNetFramework.Core.Server;
using ThinServer;
using IServer = SimpleNetFramework.Core.Server.IServer;

namespace jmicro1.Adapters
{
    public class ServerAdapter : IServer
    {
        private ThinServer.IServer _server;

        public ServerAdapter(ThinServer.IServer server)
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