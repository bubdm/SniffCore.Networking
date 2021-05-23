﻿//
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System.Net;

namespace SniffCore.Networking
{
    /// <summary>
    ///     Represents the response from a UDP broadcasting server.
    /// </summary>
    public sealed class ServerResponse
    {
        internal ServerResponse(string message, ClientConfiguration configuration, IPAddress address)
        {
            Message = message;
            Configuration = configuration;
            Address = address;
        }

        /// <summary>
        ///     The message the UDP server replied.
        /// </summary>
        public string Message { get; }

        /// <summary>
        ///     The configuration with the data what server to contact.
        /// </summary>
        public ClientConfiguration Configuration { get; }

        /// <summary>
        ///     The IP address of the UDP server.
        /// </summary>
        public IPAddress Address { get; }
    }
}