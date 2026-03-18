using CascadingConfiguration;

namespace CascadingConfigurationTests;

public sealed class ConfigurationTests {
    [TestFixture]
    public sealed class Combine {

        [TestFixture]
        public sealed class Name {
            [Test]
            public void WhenOtherIsEmpty_TakesBase() {
                var baseConfig = new Configuration("a");
                var other = new Configuration(string.Empty);

                var result = baseConfig.Combine(other);

                Assert.That(result.Name, Is.EqualTo("a"));
            }
            
            [Test]
            public void WhenBaseIsEmpty_TakesOther() {
                var baseConfig = new Configuration(string.Empty);
                var other = new Configuration("b");

                var result = baseConfig.Combine(other);

                Assert.That(result.Name, Is.EqualTo("b"));
            }
            
            [Test]
            public void WhenBothIsEmpty_UseEmpty() {
                var baseConfig = new Configuration(string.Empty);
                var other = new Configuration(string.Empty);

                var result = baseConfig.Combine(other);

                Assert.That(result.Name, Is.Empty);
            }
            
            [Test]
            public void WhenBothHaveName_TakesOther() {
                var baseConfig = new Configuration("a");
                var other = new Configuration("b");

                var result = baseConfig.Combine(other);

                Assert.That(result.Name, Is.EqualTo("b"));
            }
        }
    }
}
