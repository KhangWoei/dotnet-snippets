using CascadingConfiguration;

namespace CascadingConfigurationTests;

[TestFixture]
public sealed class ConfigurationDifferTests
{
    [TestFixture]
    public sealed class Additions
    {
        [Test]
        public void BaseHasNoChildrenAndOtherHasOneChild_CapturedAsAdded()
        {
            var child = new ChildConfiguration("child");
            var baseConfiguration = new Configuration("", []);
            var otherConfiguration = new Configuration("", [child]);

            var result = Create().Diff(baseConfiguration, otherConfiguration);

            Assert.That(result.ChildChanges.Added, Is.EquivalentTo([child]).Using(ChildConfigurationEqualityComparer.Instance));
        }

        [Test]
        public void BaseHasNoChildrenAndOtherHasMultipleChildren_AllCapturedAsAdded()
        {
            var child1 = new ChildConfiguration("child1");
            var child2 = new ChildConfiguration("child2");
            var baseConfiguration = new Configuration("", []);
            var otherConfiguration = new Configuration("", [child1, child2]);

            var result = Create().Diff(baseConfiguration, otherConfiguration);

            Assert.That(result.ChildChanges.Added, Is.EquivalentTo([child1, child2]).Using(ChildConfigurationEqualityComparer.Instance));
        }

        [Test]
        public void BaseHasChildrenAndOtherHasAdditionalNewChild_NewChildCapturedAsAdded()
        {
            var child1 = new ChildConfiguration("child1");
            var child2 = new ChildConfiguration("child2");
            var baseConfiguration = new Configuration("", [child1]);
            var otherConfiguration = new Configuration("", [child1, child2]);

            var result = Create().Diff(baseConfiguration, otherConfiguration);

            Assert.That(result.ChildChanges.Added, Is.EquivalentTo([child2]).Using(ChildConfigurationEqualityComparer.Instance));
        }
    }

    [TestFixture]
    public sealed class Deletions
    {
        [Test]
        public void BaseHasChildrenAndOtherRemovedChildren_CapturedAsDeletion()
        {
            var deletedConfiguration = new ChildConfiguration("child");
            var baseConfiguration = new Configuration("", new[]
            {
                deletedConfiguration
            });
            var otherConfiguration = new Configuration("", []);

            var result = Create().Diff(baseConfiguration, otherConfiguration);

            Assert.That(result.ChildChanges.Deleted, Is.EquivalentTo([deletedConfiguration]).Using(ChildConfigurationEqualityComparer.Instance));
        }

        [Test]
        public void BaseHasMultipleChildrenAndOtherRemovesOne_RemovedChildCapturedAsDeletion()
        {
            var child1 = new ChildConfiguration("child1");
            var child2 = new ChildConfiguration("child2");
            var baseConfiguration = new Configuration("", [child1, child2]);
            var otherConfiguration = new Configuration("", [child1]);

            var result = Create().Diff(baseConfiguration, otherConfiguration);

            Assert.That(result.ChildChanges.Deleted, Is.EquivalentTo([child2]).Using(ChildConfigurationEqualityComparer.Instance));
        }
    }

    [TestFixture]
    public sealed class Updates
    {
        [Test]
        public void ChildPresentInBothBaseAndOther_CapturedAsUpdate()
        {
            var child = new ChildConfiguration("child");
            var baseConfiguration = new Configuration("", [child]);
            var otherConfiguration = new Configuration("", [child]);

            var result = Create().Diff(baseConfiguration, otherConfiguration);

            Assert.That(result.ChildChanges.Updated, Has.Count.EqualTo(1));
        }

        [Test]
        public void MultipleChildrenPresentInBothBaseAndOther_AllCapturedAsUpdate()
        {
            var child1 = new ChildConfiguration("child1");
            var child2 = new ChildConfiguration("child2");
            var baseConfiguration = new Configuration("", [child1, child2]);
            var otherConfiguration = new Configuration("", [child1, child2]);

            var result = Create().Diff(baseConfiguration, otherConfiguration);

            Assert.That(result.ChildChanges.Updated, Has.Count.EqualTo(2));
        }
    }

    [TestFixture]
    public sealed class Unchanged
    {
        [Test]
        public void BothConfigurationsHaveNoChildren_NoChangesCapture()
        {
            var baseConfiguration = new Configuration("", []);
            var otherConfiguration = new Configuration("", []);

            var result = Create().Diff(baseConfiguration, otherConfiguration);

            Assert.That(result.ChildChanges.Added, Is.Empty);
            Assert.That(result.ChildChanges.Deleted, Is.Empty);
            Assert.That(result.ChildChanges.Updated, Is.Empty);
        }

        [Test]
        public void ChildPresentInBothBaseAndOther_NotCapturedAsAddedOrDeleted()
        {
            var child = new ChildConfiguration("child");
            var baseConfiguration = new Configuration("", [child]);
            var otherConfiguration = new Configuration("", [child]);

            var result = Create().Diff(baseConfiguration, otherConfiguration);

            Assert.That(result.ChildChanges.Added, Is.Empty);
            Assert.That(result.ChildChanges.Deleted, Is.Empty);
        }
    }

    private static ConfigurationDiffer Create() => new (new ChildConfigurationDiffer());
}