//
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using System.Collections.Generic;
using SniffCore.Networking.Broadcast.Internal;

namespace SniffCore.Networking.Broadcast
{
    /// <summary>
    ///     Provides possibilities to launch UDP broadcasting server and send messages to them on the network.
    /// </summary>
    /// <example>
    ///     <![CDATA[
    /// // Server
    /// private readonly IBroadcasting _broadcasting;
    /// private ServerToken _token;
    /// 
    /// private void StartServer()
    /// {
    ///     _token = _broadcasting.Start(new ServerConfiguration(37455, "Hello Client", s => s == "Hello Server"));
    ///     _broadcasting.ClientMessageReceiving += OnClientMessageReceiving;
    ///     _broadcasting.ClientMessageReceived += OnClientMessageReceived;
    /// }
    /// 
    /// private void OnClientMessageReceiving(object sender, ClientMessageReceivingEventArgs e)
    /// {
    ///     Console.WriteLine($"'{e.Address}' has send '{e.Message}' and we will reply '{e.Configuration.ResponseMessage}'");
    /// }
    /// 
    /// private void OnClientMessageReceived(object sender, ClientMessageReceivedEventArgs e)
    /// {
    ///     Console.WriteLine($"'{e.Address}' has send '{e.Message}' and we replied with '{e.Configuration.ResponseMessage}'");
    /// }
    /// 
    /// private void StopServer()
    /// {
    ///     _broadcasting.Dispose(_token);
    /// }
    /// 
    /// // Client
    /// public void SendBroadcast(IBroadcasting broadcasting)
    /// {
    ///     var configuration = new ClientConfiguration(37455, "Hello Server", TimeSpan.FromSeconds(10));
    ///     broadcasting.Send(configuration, response =>
    ///     {
    ///         Console.WriteLine($"'{response.Address}' as reply to the '{response.Configuration.Message}' with '{response.Message}'");
    ///     });
    /// }
    /// ]]>
    /// </example>
    public sealed class Broadcasting : IBroadcasting
    {
        private readonly Dictionary<ServerToken, BroadcastServer> _servers;

        /// <summary>
        ///     Creates a new instance of <see cref="Broadcasting" />.
        /// </summary>
        public Broadcasting()
        {
            _servers = new Dictionary<ServerToken, BroadcastServer>();
        }

        /// <summary>
        ///     Starts a new UDP broadcasting server.
        /// </summary>
        /// <param name="configuration">The configuration of the UDP broadcasting server.</param>
        /// <returns>The token which represents the started UDP server. Use on Dispose to stop it.</returns>
        /// <example>
        ///     <![CDATA[
        /// private readonly IBroadcasting _broadcasting;
        /// private ServerToken _token;
        /// 
        /// private void StartServer()
        /// {
        ///     _token = _broadcasting.Start(new ServerConfiguration(37455, "Hello Client", s => s == "Hello Server"));
        ///     _broadcasting.ClientMessageReceiving += OnClientMessageReceiving;
        ///     _broadcasting.ClientMessageReceived += OnClientMessageReceived;
        /// }
        /// 
        /// private void OnClientMessageReceiving(object sender, ClientMessageReceivingEventArgs e)
        /// {
        ///     Console.WriteLine($"'{e.Address}' has send '{e.Message}' and we will reply '{e.Configuration.ResponseMessage}'");
        /// }
        /// 
        /// private void OnClientMessageReceived(object sender, ClientMessageReceivedEventArgs e)
        /// {
        ///     Console.WriteLine($"'{e.Address}' has send '{e.Message}' and we replied with '{e.Configuration.ResponseMessage}'");
        /// }
        /// 
        /// private void StopServer()
        /// {
        ///     _broadcasting.Dispose(_token);
        /// }
        /// ]]>
        /// </example>
        /// <exception cref="ArgumentNullException">configuration is null.</exception>
        public ServerToken Start(ServerConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var token = new ServerToken();
            var server = new BroadcastServer();
            server.ClientMessageReceiving += OnClientMessageReceiving;
            server.ClientMessageReceived += OnClientMessageReceived;
            server.Run(configuration);
            _servers[token] = server;
            return token;
        }

        /// <summary>
        ///     Raised if an accepted client message came in. Its not replied yet.
        /// </summary>
        public event EventHandler<ClientMessageReceivingEventArgs> ClientMessageReceiving;

        /// <summary>
        ///     Raised if an accepted client message came in. Its replied already.
        /// </summary>
        public event EventHandler<ClientMessageReceivedEventArgs> ClientMessageReceived;

        /// <summary>
        ///     Stops all created UDP broadcasting servers.
        /// </summary>
        public void Dispose()
        {
            foreach (var (_, value) in _servers)
            {
                value.ClientMessageReceiving -= OnClientMessageReceiving;
                value.ClientMessageReceived -= OnClientMessageReceived;
                value.Dispose();
            }
        }

        /// <summary>
        ///     Sends a message to a UDP broadcasting server in the network.
        /// </summary>
        /// <param name="configuration">The configuration how and what to send.</param>
        /// <param name="callback">The callback if the server replied.</param>
        /// <example>
        ///     <![CDATA[
        /// public void SendBroadcast(IBroadcasting broadcasting)
        /// {
        ///     var configuration = new ClientConfiguration(37455, "Hello Server", TimeSpan.FromSeconds(10));
        ///     broadcasting.Send(configuration, response =>
        ///     {
        ///         Console.WriteLine($"'{response.Address}' as reply to the '{response.Configuration.Message}' with '{response.Message}'");
        ///     });
        /// }
        /// ]]>
        /// </example>
        /// <exception cref="ArgumentNullException">configuration is null.</exception>
        /// <exception cref="ArgumentNullException">callback is null.</exception>
        public void Send(ClientConfiguration configuration, Action<ServerResponse> callback)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            new BroadcastClient().Run(configuration, callback);
        }

        /// <summary>
        ///     Stops the UDP broadcasting server started by the given token.
        /// </summary>
        /// <param name="token">The token of the UDP broadcasting server to stop.</param>
        /// <exception cref="ArgumentNullException">token is null.</exception>
        public void Dispose(ServerToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            if (!_servers.TryGetValue(token, out var server))
                return;

            server.ClientMessageReceiving -= OnClientMessageReceiving;
            server.ClientMessageReceived -= OnClientMessageReceived;
            server.Dispose();
            _servers.Remove(token);
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