//
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using System.Net;

namespace SniffCore.Networking
{
    /// <summary>
    ///     Raised if a client message just came in and was allowed. Reply is not sent yet.
    /// </summary>
    public sealed class ClientMessageReceivingEventArgs : EventArgs
    {
        internal ClientMessageReceivingEventArgs(IPAddress address, string message, ServerConfiguration configuration)
        {
            Address = address;
            Message = message;
            Configuration = configuration;
        }

        /// <summary>
        ///     The IP address of the client.
        /// </summary>
        public IPAddress Address { get; }

        /// <summary>
        ///     The message the client has sent.
        /// </summary>
        public string Message { get; }

        /// <summary>
        ///     The configuration of the UDP server who got a message.
        /// </summary>
        public ServerConfiguration Configuration { get; }
    }
}