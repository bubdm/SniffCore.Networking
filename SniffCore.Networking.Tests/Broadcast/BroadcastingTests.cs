using System;
using NUnit.Framework;
using SniffCore.Networking.Broadcast;

namespace SniffCore.Networking.Tests.Broadcast
{
    // Tests for send and receive cannot be done in unit tests
    [TestFixture]
    public class BroadcastingTests
    {
        [SetUp]
        public void Setup()
        {
            _target = new Broadcasting();
        }

        [TearDown]
        public void Teardown()
        {
            _target.Dispose();
        }

        private Broadcasting _target;

        [Test]
        public void Start_CalledWithNull_ThrowsException()
        {
            ServerConfiguration configuration = null;

            var act = new TestDelegate(() => _target.Start(configuration));

            Assert.Throws<ArgumentNullException>(act);
        }

        [Test]
        public void Dispose_CalledWithNull_ThrowsException()
        {
            ServerToken serverToken = null;

            var act = new TestDelegate(() => _target.Dispose(serverToken));

            Assert.Throws<ArgumentNullException>(act);
        }

        [Test]
        public void Send_CalledWithClientDataNull_ThrowsException()
        {
            ClientConfiguration configuration = null;
            Action<ServerResponse> callback = response => { };

            var act = new TestDelegate(() => _target.Send(configuration, callback));

            Assert.Throws<ArgumentNullException>(act);
        }

        [Test]
        public void Send_CalledWithCallbackNull_ThrowsException()
        {
            var configuration = new ClientConfiguration(12345, "Hello Server", TimeSpan.FromSeconds(10));
            Action<ServerResponse> callback = null;

            var act = new TestDelegate(() => _target.Send(configuration, callback));

            Assert.Throws<ArgumentNullException>(act);
        }
    }
}