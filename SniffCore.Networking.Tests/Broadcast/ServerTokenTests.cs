using NUnit.Framework;
using SniffCore.Networking.Broadcast;

namespace SniffCore.Networking.Tests.Broadcast
{
    [TestFixture]
    public class ServerTokenTests
    {
        [SetUp]
        public void Setup()
        {
            _target = new ServerToken();
        }

        private ServerToken _target;

        [Test]
        public void Equals_ToSameToken_ReturnsTrue()
        {
            var otherToken = _target;

            var result = _target.Equals(otherToken);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Equals_ToSameObjectToken_ReturnsTrue()
        {
            object otherToken = _target;

            var result = _target.Equals(otherToken);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Equals_ToDifferentToken_ReturnsFalse()
        {
            var otherToken = new ServerToken();

            var result = _target.Equals(otherToken);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_ToDifferentObjectToken_ReturnsFalse()
        {
            object otherToken = new ServerToken();

            var result = _target.Equals(otherToken);

            Assert.That(result, Is.False);
        }
    }
}