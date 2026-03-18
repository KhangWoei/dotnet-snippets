using CascadingConfiguration.Configurations.Diffing;

namespace CascadingConfiguration.Configurations.Child;

public sealed record ChildConfigurationDifference(FieldChange<string> Name, FieldChange<bool?> Enabled, FieldChange<bool?> Disabled);