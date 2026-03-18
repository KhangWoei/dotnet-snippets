using CascadingConfiguration;

namespace CascadingConfigurationTests;

[TestFixture]
public sealed class ConfigurationTests
{
    [TestFixture]
    public sealed class Combine {
        [Test]
        public void WhenOtherIsNull_ReturnsBase()
        {
            var baseConfig = new Configuration("a");

            var result = baseConfig.Combine(null);

            Assert.That(result, Is.EqualTo(baseConfig).Using(ConfigurationEqualityComparer.Instance));
        }
    }
}