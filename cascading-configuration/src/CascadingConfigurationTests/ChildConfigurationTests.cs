using CascadingConfiguration;

namespace CascadingConfigurationTests;

public sealed class ChildConfigurationTests {
    [TestFixture]
    public sealed class Combine {

        [TestFixture]
        public sealed class Enabled {
            [Test]
            public void WhenBaseSet_OtherNull_TakesBase() {
                var baseConfig = new ChildConfiguration(true, null);
                var other = new ChildConfiguration(null, null);

                var result = baseConfig.Combine(other);

                Assert.That(result, Is.EqualTo(new ChildConfiguration(true, null)).Using(ChildConfigurationEqualityComparer.Instance));
            }

            [Test]
            public void WhenBaseNull_OtherSet_TakesOther() {
                var baseConfig = new ChildConfiguration(null, null);
                var other = new ChildConfiguration(false, null);

                var result = baseConfig.Combine(other);

                Assert.That(result, Is.EqualTo(new ChildConfiguration(false, null)).Using(ChildConfigurationEqualityComparer.Instance));
            }

            [Test]
            public void WhenBothSet_OtherTakesPrecedence() {
                var baseConfig = new ChildConfiguration(true, null);
                var other = new ChildConfiguration(false, null);

                var result = baseConfig.Combine(other);

                Assert.That(result, Is.EqualTo(new ChildConfiguration(false, null)).Using(ChildConfigurationEqualityComparer.Instance));
            }

            [Test]
            public void WhenBothNull_ResultIsNull() {
                var baseConfig = new ChildConfiguration(null, null);
                var other = new ChildConfiguration(null, null);

                var result = baseConfig.Combine(other);

                Assert.That(result, Is.EqualTo(new ChildConfiguration(null, null)).Using(ChildConfigurationEqualityComparer.Instance));
            }
        }

        [TestFixture]
        public sealed class Disabled {
            [Test]
            public void WhenBaseSet_OtherNull_TakesBase() {
                var baseConfig = new ChildConfiguration(null, true);
                var other = new ChildConfiguration(null, null);

                var result = baseConfig.Combine(other);

                Assert.That(result, Is.EqualTo(new ChildConfiguration(null, true)).Using(ChildConfigurationEqualityComparer.Instance));
            }

            [Test]
            public void WhenBaseNull_OtherSet_TakesOther() {
                var baseConfig = new ChildConfiguration(null, null);
                var other = new ChildConfiguration(null, false);

                var result = baseConfig.Combine(other);

                Assert.That(result, Is.EqualTo(new ChildConfiguration(null, false)).Using(ChildConfigurationEqualityComparer.Instance));
            }

            [Test]
            public void WhenBothSet_OtherTakesPrecedence() {
                var baseConfig = new ChildConfiguration(null, true);
                var other = new ChildConfiguration(null, false);

                var result = baseConfig.Combine(other);

                Assert.That(result, Is.EqualTo(new ChildConfiguration(null, false)).Using(ChildConfigurationEqualityComparer.Instance));
            }

            [Test]
            public void WhenBothNull_ResultIsNull() {
                var baseConfig = new ChildConfiguration(null, null);
                var other = new ChildConfiguration(null, null);

                var result = baseConfig.Combine(other);

                Assert.That(result, Is.EqualTo(new ChildConfiguration(null, null)).Using(ChildConfigurationEqualityComparer.Instance));
            }
        }
    }
}
