using CascadingConfiguration.Configurations.Child;
using CascadingConfiguration.Configurations.Diffing;

namespace CascadingConfiguration.Configurations;

public sealed class ConfigurationDifference(
    FieldChange<string> nameChange,
    ChildConfigurationCollectionDifference childChanges)
{
    public FieldChange<string> NameChange { get; } = nameChange;

    public ChildConfigurationCollectionDifference ChildChanges { get; } = childChanges;

}