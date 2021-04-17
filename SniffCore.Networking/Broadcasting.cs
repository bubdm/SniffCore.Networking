using System;
using System.Collections.Generic;

namespace SniffCore.Networking
{
    /// <inheritdoc />
    public class Broadcasting : IBroadcasting
    {
        private readonly Dictionary<ServerToken, BroadcastServer> _servers;

        /// <summary>
        ///     Creates a new instance of Broadcasting.
        /// </summary>
        public Broadcasting()
        {
            _servers = new Dictionary<ServerToken, BroadcastServer>();
        }

        /// <inheritdoc />
        public ServerToken Start(ServerConfiguration configuration)
        {
            var token = new ServerToken();
            var server = new BroadcastServer();
            server.ClientMessageReceiving += OnClientMessageReceiving;
            server.ClientMessageReceived += OnClientMessageReceived;
            server.Run(configuration);
            _servers[token] = server;
            return token;
        }

        /// <inheritdoc />
        public event EventHandler<ClientMessageReceivingEventArgs> ClientMessageReceiving;

        /// <inheritdoc />
        public event EventHandler<ClientMessageReceivedEventArgs> ClientMessageReceived;

        /// <summary>
        ///     Stops all created UDP broadcasting servers.
        /// </summary>
        public void Dispose()
        {
            foreach (var server in _servers)
            {
                server.Value.ClientMessageReceiving -= OnClientMessageReceiving;
                server.Value.ClientMessageReceived -= OnClientMessageReceived;
                server.Value.Dispose();
            }
        }

        /// <inheritdoc />
        public void Send(ClientConfiguration configuration, Action<ServerResponse> callback)
        {
            new BroadcastClient().Run(configuration, callback);
        }

        /// <inheritdoc />
        public void Dispose(ServerToken token)
        {
            if (_servers.TryGetValue(token, out var server))
            {
                server.ClientMessageReceiving -= OnClientMessageReceiving;
                server.ClientMessageReceived -= OnClientMessageReceived;
                server.Dispose();
                _servers.Remove(token);
            }
        }

        private void OnClientMessageReceiving(object sender, ClientMessageReceivingEventArgs e)
        {
            ClientMessageReceiving?.Invoke(sender, e);
        }

        private void OnClientMessageReceived(object sender, ClientMessageReceivedEventArgs e)
        {
            ClientMessageReceived?.Invoke(sender, e);
        }
    }
}