using CascadingConfiguration.Configuration.Child;

namespace CascadingConfigurationTests.Configuration.Child;

[TestFixture]
internal sealed class ChildConfigurationCombinerTests
{
    [TestFixture]
    public sealed class Name
    {
        [Test]
        public void WhenOtherIsEmpty_TakesBase()
        {
            var baseConfig = new ChildConfiguration("base");
            var other = new ChildConfiguration("");

            var result = Create().Combine(baseConfig, other);
            
            Assert.That(result.Name, Is.EqualTo(baseConfig.Name));
        }

        [Test]
        public void WhenBaseIsEmpty_TakesOther()
        {
            var baseConfig = new ChildConfiguration("");
            var other = new ChildConfiguration("other");

            var result = Create().Combine(baseConfig, other);

            Assert.That(result.Name, Is.EqualTo(other.Name));
        }

        [Test]
        public void WhenBothIsEmpty_UseEmpty()
        {
            var baseConfig = new ChildConfiguration(string.Empty);
            var other = new ChildConfiguration(string.Empty);

            var result = Create().Combine(baseConfig, other);

            Assert.That(result.Name, Is.Empty);
        }

        [Test]
        public void WhenBothHaveName_TakesOther()
        {
            var baseConfig = new ChildConfiguration("base");
            var other = new ChildConfiguration("other");

            var result = Create().Combine(baseConfig, other);

            Assert.That(result.Name, Is.EqualTo(other.Name));
        }
    }

    [TestFixture]
    public sealed class Enabled
    {
        [Test]
        public void WhenBaseSet_OtherNull_TakesBase()
        {
            var baseConfig = new ChildConfiguration("", true, null);
            var other = new ChildConfiguration("", null, null);

            var result = Create().Combine(baseConfig, other);

            Assert.That(result,
                Is.EqualTo(new ChildConfiguration("", true, null))
                    .Using(ChildConfigurationEqualityComparer.Instance));
        }

        [Test]
        public void WhenBaseNull_OtherSet_TakesOther()
        {
            var baseConfig = new ChildConfiguration("", null, null);
            var other = new ChildConfiguration("", false, null);

            var result = Create().Combine(baseConfig, other);

            Assert.That(result,
                Is.EqualTo(new ChildConfiguration("", false, null))
                    .Using(ChildConfigurationEqualityComparer.Instance));
        }

        [Test]
        public void WhenBothSet_OtherTakesPrecedence()
        {
            var baseConfig = new ChildConfiguration("", true, null);
            var other = new ChildConfiguration("", false, null);

            var result = Create().Combine(baseConfig, other);

            Assert.That(result,
                Is.EqualTo(new ChildConfiguration("", false, null))
                    .Using(ChildConfigurationEqualityComparer.Instance));
        }

        [Test]
        public void WhenBothNull_ResultIsNull()
        {
            var baseConfig = new ChildConfiguration("", null, null);
            var other = new ChildConfiguration("", null, null);

            var result = Create().Combine(baseConfig, other);

            Assert.That(result,
                Is.EqualTo(new ChildConfiguration("", null, null))
                    .Using(ChildConfigurationEqualityComparer.Instance));
        }
    }

    [TestFixture]
    public sealed class Disabled
    {
        [Test]
        public void WhenBaseSet_OtherNull_TakesBase()
        {
            var baseConfig = new ChildConfiguration("", null, true);
            var other = new ChildConfiguration("", null, null);

            var result = Create().Combine(baseConfig, other);

            Assert.That(result,
                Is.EqualTo(new ChildConfiguration("", null, true))
                    .Using(ChildConfigurationEqualityComparer.Instance));
        }

        [Test]
        public void WhenBaseNull_OtherSet_TakesOther()
        {
            var baseConfig = new ChildConfiguration("", null, null);
            var other = new ChildConfiguration("", null, false);

            var result = Create().Combine(baseConfig, other);

            Assert.That(result,
                Is.EqualTo(new ChildConfiguration("", null, false))
                    .Using(ChildConfigurationEqualityComparer.Instance));
        }

        [Test]
        public void WhenBothSet_OtherTakesPrecedence()
        {
            var baseConfig = new ChildConfiguration("", null, true);
            var other = new ChildConfiguration("", null, false);

            var result = Create().Combine(baseConfig, other);

            Assert.That(result,
                Is.EqualTo(new ChildConfiguration("", null, false))
                    .Using(ChildConfigurationEqualityComparer.Instance));
        }

        [Test]
        public void WhenBothNull_ResultIsNull()
        {
            var baseConfig = new ChildConfiguration("", null, null);
            var other = new ChildConfiguration("", null, null);

            var result = Create().Combine(baseConfig, other);

            Assert.That(result,
                Is.EqualTo(new ChildConfiguration("", null, null))
                    .Using(ChildConfigurationEqualityComparer.Instance));
        }
    }

    private static ChildConfigurationCombiner Create() => new ();
}