using CascadingConfiguration.Configuration.Child;

namespace CascadingConfigurationTests.Configuration.Child;

[TestFixture]
public sealed class ChildConfigurationTests
{
    [TestFixture]
    public sealed class Combine {
        [Test]
        public void WhenOtherIsNull_ReturnsBase()
        {
            var baseConfig = new ChildConfiguration("a");

            var result = baseConfig.Combine(null);

            Assert.That(result, Is.EqualTo(baseConfig).Using(ChildConfigurationEqualityComparer.Instance));
        }
    }
}