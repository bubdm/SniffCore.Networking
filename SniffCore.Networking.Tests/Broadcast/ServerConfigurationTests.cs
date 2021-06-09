using System;
using NUnit.Framework;
using SniffCore.Networking.Broadcast;

namespace SniffCore.Networking.Tests.Broadcast
{
    [TestFixture]
    public class ServerConfigurationTests
    {
        [Test]
        public void Ctor_CalledWithNegativePort_ThrowsException()
        {
            var port = -1;
            var message = "Hello Server";
            Func<string, bool> filter = s => true;

            var action = new TestDelegate(() => _ = new ServerConfiguration(port, message, filter));

            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void Ctor_CalledWithZeroPort_ThrowsException()
        {
            var port = 0;
            var message = "Hello Server";
            Func<string, bool> filter = s => true;

            var action = new TestDelegate(() => _ = new ServerConfiguration(port, message, filter));

            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void Ctor_CalledWithNullMessage_ThrowsException()
        {
            var port = 12345;
            string message = null;
            Func<string, bool> filter = s => true;

            var action = new TestDelegate(() => _ = new ServerConfiguration(port, message, filter));

            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void Ctor_CalledWithEmptyMessage_ThrowsException()
        {
            var port = 12345;
            var message = string.Empty;
            Func<string, bool> filter = s => true;

            var action = new TestDelegate(() => _ = new ServerConfiguration(port, message, filter));

            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void Ctor_CalledWithWhitespaceMessage_ThrowsException()
        {
            var port = 12345;
            var message = "  ";
            Func<string, bool> filter = s => true;

            var action = new TestDelegate(() => _ = new ServerConfiguration(port, message, filter));

            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void Ctor_CalledWithNullFilter_ThrowsException()
        {
            var port = 12345;
            var message = "Hello Client";
            Func<string, bool> filter = null;

            var action = new TestDelegate(() => _ = new ServerConfiguration(port, message, filter));

            Assert.Throws<ArgumentNullException>(action);
        }
    }
}