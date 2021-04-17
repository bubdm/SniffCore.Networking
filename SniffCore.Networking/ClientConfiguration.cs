using System;

namespace SniffCore.Networking
{
    /// <summary>
    ///     The configuration data of the broadcasting client what to send to an UDP broadcasting server on which port.
    /// </summary>
    public sealed class ClientConfiguration
    {
        /// <summary>
        ///     Creates a new instance of ClientConfiguration
        /// </summary>
        /// <param name="port">The port to send the UDP message on.</param>
        /// <param name="message">The message to send on the UDP.</param>
        /// <param name="timeout">The timeout how long to wait for a UDP server response.</param>
        public ClientConfiguration(int port, string message, TimeSpan timeout)
        {
            Port = port;
            Message = message;
            Timeout = timeout;
        }

        /// <summary>
        ///     The port to send the UDP message on.
        /// </summary>
        public int Port { get; }

        /// <summary>
        ///     The message to send on the UDP.
        /// </summary>
        public string Message { get; }

        /// <summary>
        ///     The timeout how long to wait for a UDP server response.
        /// </summary>
        public TimeSpan Timeout { get; }
    }
}