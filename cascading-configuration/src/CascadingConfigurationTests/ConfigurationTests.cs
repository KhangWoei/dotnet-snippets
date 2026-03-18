using CascadingConfiguration;

namespace CascadingConfigurationTests;

[TestFixture]
public sealed class ConfigurationTests
{
    [TestFixture]
    public sealed class Combine
    {
        [TestFixture]
        public sealed class Name
        {
            [Test]
            public void WhenOtherIsEmpty_TakesBase()
            {
                var baseConfig = new Configuration("a");
                var other = new Configuration(string.Empty);

                var result = baseConfig.Combine(other);

                Assert.That(result.Name, Is.EqualTo("a"));
            }

            [Test]
            public void WhenBaseIsEmpty_TakesOther()
            {
                var baseConfig = new Configuration(string.Empty);
                var other = new Configuration("b");

                var result = baseConfig.Combine(other);

                Assert.That(result.Name, Is.EqualTo("b"));
            }

            [Test]
            public void WhenBothIsEmpty_UseEmpty()
            {
                var baseConfig = new Configuration(string.Empty);
                var other = new Configuration(string.Empty);

                var result = baseConfig.Combine(other);

                Assert.That(result.Name, Is.Empty);
            }

            [Test]
            public void WhenBothHaveName_TakesOther()
            {
                var baseConfig = new Configuration("a");
                var other = new Configuration("b");

                var result = baseConfig.Combine(other);

                Assert.That(result.Name, Is.EqualTo("b"));
            }
        }

        [TestFixture]
        public sealed class Child
        {
            [Test]
            public void WhenBaseHasChild_OtherDoesNot_TakesBaseChild()
            {
                var baseChild = new ChildConfiguration("baseChild", true);
                var baseConfig = new Configuration("name", [baseChild]);
                var other = new Configuration("name");

                var result = baseConfig.Combine(other);

                Assert.That(result.Childs,
                    Is.EquivalentTo([baseChild]).Using(ChildConfigurationEqualityComparer.Instance));
            }

            [Test]
            public void WhenOtherHasChild_BaseDoesNot_TakesOtherChild()
            {
                var baseConfig = new Configuration("name");
                var otherChild = new ChildConfiguration("otherChid");
                var other = new Configuration("name", [otherChild]);

                var result = baseConfig.Combine(other);

                Assert.That(result.Childs,
                    Is.EquivalentTo([otherChild]).Using(ChildConfigurationEqualityComparer.Instance));
            }

            [Test]
            public void WhenNeitherHasChild_ResultHasNoChild()
            {
                var baseConfig = new Configuration("name");
                var other = new Configuration("name");

                var result = baseConfig.Combine(other);

                Assert.That(result.Childs, Is.Empty);
            }

            [Test]
            public void WhenBothHaveChildWithTheSameName_ChildrenAreCombined()
            {
                var baseChild = new ChildConfiguration("sharedChild", true);
                var baseConfig = new Configuration("name", [baseChild]);

                var otherChild = new ChildConfiguration("sharedChild", false);
                var other = new Configuration("name", [otherChild]);

                var result = baseConfig.Combine(other);

                var expected = baseChild.Combine(otherChild);
                Assert.That(result.Childs,
                    Is.EquivalentTo([expected]).Using(ChildConfigurationEqualityComparer.Instance));
            }

            [Test]
            public void WhenBothHaveChildWithDifferentName_ChildrenAreAppended()
            {
                var baseChild = new ChildConfiguration("baseChild");
                var baseConfig = new Configuration("name", [baseChild]);

                var otherChild = new ChildConfiguration("otherChild");
                var other = new Configuration("name", [otherChild]);

                var result = baseConfig.Combine(other);

                var expected = new[] { baseChild, otherChild };
                Assert.That(result.Childs,
                    Is.EquivalentTo(expected).Using(ChildConfigurationEqualityComparer.Instance));
            }
        }
    }
}