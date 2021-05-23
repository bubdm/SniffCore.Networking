//
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;

namespace SniffCore.Networking
{
    /// <summary>
    ///     Provides possibilities to launch UDP broadcasting server and send messages to them on the network.
    /// </summary>
    /// <remarks>
    /// Best to use in an IOC container. IBroadcasting -> Broadcasting.
    /// </remarks>
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
    public interface IBroadcasting : IDisposable
    {
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
        ServerToken Start(ServerConfiguration configuration);

        /// <summary>
        ///     Raised if an accepted client message came in. Its not replied yet.
        /// </summary>
        event EventHandler<ClientMessageReceivingEventArgs> ClientMessageReceiving;

        /// <summary>
        ///     Raised if an accepted client message came in. Its replied already.
        /// </summary>
        event EventHandler<ClientMessageReceivedEventArgs> ClientMessageReceived;

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
        void Send(ClientConfiguration configuration, Action<ServerResponse> callback);

        /// <summary>
        ///     Stops the UDP broadcasting server started by the given token.
        /// </summary>
        /// <param name="token">The token of the UDP broadcasting server to stop.</param>
        void Dispose(ServerToken token);
    }
}