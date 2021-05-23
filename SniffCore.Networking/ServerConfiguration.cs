//
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;

namespace SniffCore.Networking
{
    public sealed class ServerConfiguration
    {
        public ServerConfiguration(int port, string responseMessage, Func<string, bool> filter)
        {
            Port = port;
            ResponseMessage = responseMessage;
            Filter = filter;
        }

        public int Port { get; }

        public string ResponseMessage { get; }

        public Func<string, bool> Filter { get; }
    }
}