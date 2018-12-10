using NUnit.Framework;
using DungeonsAndDragons.Models;

namespace Tests
{
    [TestFixture]
    public class TestingTests
    {

        [Test]
        public void ReturnsFalseForUniqueIntList()
        {
            var result = TestModel.Number(4);

            Assert.AreEqual(4, result);
        }


    }
}