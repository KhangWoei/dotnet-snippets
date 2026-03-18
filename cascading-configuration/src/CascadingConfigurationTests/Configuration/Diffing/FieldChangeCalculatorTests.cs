using CascadingConfiguration.Configuration.Diffing;

namespace CascadingConfigurationTests.Configuration.Diffing;

[TestFixture]
public sealed class FieldChangeCalculatorTests
{
    [TestFixture]
    public sealed class StringTests
    {
        [Test]
        public void WhenBothSame_CapturedAsUnchanged()
        {
            var result = FieldChangeCalculator<string>.Calculate("Field", "value", "value");
            Assert.That(result.Type, Is.EqualTo(ChangeType.Unchanged));
        }

        [Test]
        public void WhenBothNullOrEmpty_CapturedAsUnchanged()
        {
            var result = FieldChangeCalculator<string>.Calculate("Field", "", null);
            Assert.That(result.Type, Is.EqualTo(ChangeType.Unchanged));
        }

        [Test]
        public void WhenOldEmptyAndNewSet_CapturedAsAdded()
        {
            var result = FieldChangeCalculator<string>.Calculate("Field", "", "new");
            Assert.That(result.Type, Is.EqualTo(ChangeType.Added));
        }

        [Test]
        public void WhenOldNullAndNewSet_CapturedAsAdded()
        {
            var result = FieldChangeCalculator<string>.Calculate("Field", null, "new");
            Assert.That(result.Type, Is.EqualTo(ChangeType.Added));
        }

        [Test]
        public void WhenOldSetAndNewEmpty_CapturedAsDeleted()
        {
            var result = FieldChangeCalculator<string>.Calculate("Field", "old", "");
            Assert.That(result.Type, Is.EqualTo(ChangeType.Deleted));
        }

        [Test]
        public void WhenOldSetAndNewNull_CapturedAsDeleted()
        {
            var result = FieldChangeCalculator<string>.Calculate("Field", "old", null);
            Assert.That(result.Type, Is.EqualTo(ChangeType.Deleted));
        }

        [Test]
        public void WhenBothSetAndDifferent_CapturedAsUpdated()
        {
            var result = FieldChangeCalculator<string>.Calculate("Field", "old", "new");
            Assert.That(result.Type, Is.EqualTo(ChangeType.Updated));
        }

        [Test]
        public void OriginalValuesArePreserved()
        {
            var result = FieldChangeCalculator<string>.Calculate("Field", "", "new");
            Assert.That(result.Old, Is.EqualTo(""));
            Assert.That(result.New, Is.EqualTo("new"));
        }
    }

    [TestFixture]
    public sealed class NullableBoolTests
    {
        [Test]
        public void WhenBothSame_CapturedAsUnchanged()
        {
            var result = FieldChangeCalculator<bool?>.Calculate("Field", true, true);
            Assert.That(result.Type, Is.EqualTo(ChangeType.Unchanged));
        }

        [Test]
        public void WhenBothNull_CapturedAsUnchanged()
        {
            var result = FieldChangeCalculator<bool?>.Calculate("Field", null, null);
            Assert.That(result.Type, Is.EqualTo(ChangeType.Unchanged));
        }

        [Test]
        public void WhenOldNullAndNewSet_CapturedAsAdded()
        {
            var result = FieldChangeCalculator<bool?>.Calculate("Field", null, true);
            Assert.That(result.Type, Is.EqualTo(ChangeType.Added));
        }

        [Test]
        public void WhenOldSetAndNewNull_CapturedAsDeleted()
        {
            var result = FieldChangeCalculator<bool?>.Calculate("Field", true, null);
            Assert.That(result.Type, Is.EqualTo(ChangeType.Deleted));
        }

        [Test]
        public void WhenBothSetAndDifferent_CapturedAsUpdated()
        {
            var result = FieldChangeCalculator<bool?>.Calculate("Field", true, false);
            Assert.That(result.Type, Is.EqualTo(ChangeType.Updated));
        }
    }
}