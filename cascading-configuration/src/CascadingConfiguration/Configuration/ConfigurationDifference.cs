using CascadingConfiguration.Configuration.Child;
using CascadingConfiguration.Configuration.Diffing;

namespace CascadingConfiguration.Configuration;

public sealed class ConfigurationDifference(
    FieldChange<string> nameChange,
    ChildConfigurationCollectionDifference childChanges)
{
    public FieldChange<string> NameChange { get; } = nameChange;

    public ChildConfigurationCollectionDifference ChildChanges { get; } = childChanges;

}