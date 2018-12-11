using NUnit.Framework;
using DungeonsAndDragons.Models;

namespace Tests
{
    public class Tests
    {
        [Test]
        public void ReturnEncryptedString()
        {
            var encrypted = Encryption.EncryptPassword("test");
            Assert.That(encrypted, Is.EqualTo("2s588k/0kB31nKqs2h696g=="));
        }
    }
}