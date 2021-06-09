using System;
using NUnit.Framework;
using SniffCore.Networking.Broadcast;

namespace SniffCore.Networking.Tests.Broadcast
{
    [TestFixture]
    public class ClientConfigurationTests
    {
        [Test]
        public void Ctor_CalledWithNegativePort_ThrowsException()
        {
            var port = -1;
            var message = "Hello Server";
            var timeout = TimeSpan.FromSeconds(10);

            var action = new TestDelegate(() => _ = new ClientConfiguration(port, message, timeout));

            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void Ctor_CalledWithZeroPort_ThrowsException()
        {
            var port = 0;
            var message = "Hello Server";
            var timeout = TimeSpan.FromSeconds(10);

            var action = new TestDelegate(() => _ = new ClientConfiguration(port, message, timeout));

            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void Ctor_CalledWithNullMessage_ThrowsException()
        {
            var port = 12345;
            string message = null;
            var timeout = TimeSpan.FromSeconds(10);

            var action = new TestDelegate(() => _ = new ClientConfiguration(port, message, timeout));

            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void Ctor_CalledWithEmptyMessage_ThrowsException()
        {
            var port = 12345;
            var message = string.Empty;
            var timeout = TimeSpan.FromSeconds(10);

            var action = new TestDelegate(() => _ = new ClientConfiguration(port, message, timeout));

            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void Ctor_CalledWithWhitespaceMessage_ThrowsException()
        {
            var port = 12345;
            var message = "  ";
            var timeout = TimeSpan.FromSeconds(10);

            var action = new TestDelegate(() => _ = new ClientConfiguration(port, message, timeout));

            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void Ctor_CalledWithZeroTimespan_ThrowsException()
        {
            var port = 12345;
            var message = "  ";
            var timeout = TimeSpan.Zero;

            var action = new TestDelegate(() => _ = new ClientConfiguration(port, message, timeout));

            Assert.Throws<ArgumentException>(action);
        }
    }
}