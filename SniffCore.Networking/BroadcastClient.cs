﻿//
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SniffCore.Networking
{
    internal sealed class BroadcastClient
    {
        private UdpClient _client;
        private Timer _timer;

        internal void Run(ClientConfiguration configuration, Action<ServerResponse> callback)
        {
            _timer = new Timer(configuration.Timeout.TotalMilliseconds);
            _timer.Elapsed += OnTimerOnElapsed;

            Task.Run(() =>
            {
                try
                {
                    _client = new UdpClient();
                    var message = Encoding.ASCII.GetBytes(configuration.Message);
                    var server = new IPEndPoint(IPAddress.Any, 0);

                    _client.EnableBroadcast = true;
                    _client.Send(message, message.Length, new IPEndPoint(IPAddress.Broadcast, configuration.Port));

                    _timer.Start();
                    var responseData = _client.Receive(ref server);
                    var response = Encoding.ASCII.GetString(responseData);

                    callback(new ServerResponse(response, configuration, server.Address));

                    _client.Close();
                }
                catch
                {
                    //_server.Close which cancels the _server.Receive
                }
            });
        }

        private void OnTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Elapsed -= OnTimerOnElapsed;
            _timer.Stop();
            _client.Close();
        }
    }
}