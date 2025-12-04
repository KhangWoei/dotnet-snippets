using BitManipulation;

namespace BitManipulationTests;

[TestFixture]
public class BitsTests
{
    [TestFixture]
    public class Max 
    {
        [TestCase(1, -1, 1)]
        [TestCase(-1, 1, 1)]
        public void BetweenTwoValues(int left, int right, int expected)
        {
            var bits = new Bits();

            var actual = bits.Max(left, right);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Overflow()
        {
            var bits = new Bits();

            var actual = bits.Max(int.MinValue, int.MinValue);

            Assert.That(actual, Is.EqualTo(int.MaxValue));
        }
    }

    [TestFixture]
    public class Min
    {
        [TestCase(1, -1, -1)]
        [TestCase(-1, 1, -1)]
        public void BetweenTwoValues(int left, int right, int expected)
        {
            var bits = new Bits();

            var actual = bits.Min(left, right);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Overflow()
        {
            var bits = new Bits();

            var actual = bits.Min(int.MinValue, int.MinValue);

            Assert.That(actual, Is.EqualTo(int.MinValue));
        }
    }
}
