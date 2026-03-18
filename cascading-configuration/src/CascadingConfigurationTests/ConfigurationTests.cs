using CascadingConfiguration;

namespace CascadingConfigurationTests;

public sealed class ConfigurationTests {
    [TestFixture]
    public sealed class Combine {

        [TestFixture]
        public sealed class Name {
            [Test]
            public void WhenOtherIsEmpty_TakesBase() {
                var baseConfig = new Configuration("a", null, null);
                var other = new Configuration(string.Empty, null, null);

                var result = baseConfig.Combine(other);

                Assert.That(result.Name, Is.EqualTo("a"));
            }
            
            [Test]
            public void WhenBaseIsEmpty_TakesOther() {
                var baseConfig = new Configuration(string.Empty, null, null);
                var other = new Configuration("b", null, null);

                var result = baseConfig.Combine(other);

                Assert.That(result.Name, Is.EqualTo("b"));
            }
            
            [Test]
            public void WhenBothIsEmpty_UseEmpty() {
                var baseConfig = new Configuration(string.Empty, null, null);
                var other = new Configuration(string.Empty, null, null);

                var result = baseConfig.Combine(other);

                Assert.That(result.Name, Is.Empty);
            }
            
            [Test]
            public void WhenBothHaveName_TakesOther() {
                var baseConfig = new Configuration("a", null, null);
                var other = new Configuration("b", null, null);

                var result = baseConfig.Combine(other);

                Assert.That(result.Name, Is.EqualTo("b"));
            }
        }

        [TestFixture]
        public sealed class Child {
            [Test]
            public void WhenBaseHasChild_OtherDoesNot_TakesBaseChild() {
                var child = new ChildConfiguration(true, null);
                var baseConfig = new Configuration("name", child, null);
                var other = new Configuration("name", null, null);

                var result = baseConfig.Combine(other);

                Assert.That(result.Child, Is.EqualTo(child).Using(ChildConfigurationEqualityComparer.Instance));
            }

            [Test]
            public void WhenOtherHasChild_BaseDoesNot_TakesOtherChild() {
                var child = new ChildConfiguration(true, null);
                var baseConfig = new Configuration("name", null, null);
                var other = new Configuration("name", child, null);

                var result = baseConfig.Combine(other);

                Assert.That(result.Child, Is.EqualTo(child).Using(ChildConfigurationEqualityComparer.Instance));
            }

            [Test]
            public void WhenNeitherHasChild_ResultHasNoChild() {
                var baseConfig = new Configuration("name", null, null);
                var other = new Configuration("name", null, null);

                var result = baseConfig.Combine(other);

                Assert.That(result.Child, Is.Null);
            }

            [Test]
            public void WhenBothHaveChild_ChildrenAreCombined()
            {
                var baseChild = new ChildConfiguration(true, null);
                var baseConfig = new Configuration("name", baseChild, null);

                var otherChild = new ChildConfiguration(null, false);
                var other = new Configuration("name", otherChild, null);

                var result = baseConfig.Combine(other);

                var expected = baseChild.Combine(otherChild);
                Assert.That(result.Child, Is.EqualTo(expected).Using(ChildConfigurationEqualityComparer.Instance));
            }
        }

        [TestFixture]
        public sealed class Strings {
            [Test]
            public void WhenBothEmpty_ResultIsEmpty() {
                var baseConfig = new Configuration("name", null, []);
                var other = new Configuration("name", null, []);

                var result = baseConfig.Combine(other);

                Assert.That(result.Strings, Is.EquivalentTo(Array.Empty<string>()));
            }

            [Test]
            public void WhenBaseEmpty_TakesOtherStrings() {
                var baseConfig = new Configuration("name", null, []);
                var other = new Configuration("name", null, ["a", "b"]);

                var result = baseConfig.Combine(other);

                Assert.That(result.Strings, Is.EquivalentTo(["a", "b"]));
            }

            [Test]
            public void WhenOtherEmpty_TakesBaseStrings() {
                var baseConfig = new Configuration("name", null, ["a", "b"]);
                var other = new Configuration("name", null, []);

                var result = baseConfig.Combine(other);

                Assert.That(result.Strings, Is.EquivalentTo(["a", "b"]));
            }

            [Test]
            public void WhenDistinct_ResultIsUnion() {
                var baseConfig = new Configuration("name", null, ["a", "b"]);
                var other = new Configuration("name", null, ["c", "d"]);

                var result = baseConfig.Combine(other);

                Assert.That(result.Strings, Is.EquivalentTo(["a", "b", "c", "d"]));
            }

            [Test]
            public void WhenOverlapping_NoDuplicatesInResult() {
                var baseConfig = new Configuration("name", null, ["a", "b"]);
                var other = new Configuration("name", null, ["b", "c"]);

                var result = baseConfig.Combine(other);

                var expected = new Configuration("name", null, ["a", "b", "c"]);
                Assert.That(result, Is.EqualTo(expected).Using(ConfigurationEqualityComparer.Instance));
                Assert.That(result.Strings, Is.EquivalentTo(["a", "b", "c"]));
            }
        }
    }
}
