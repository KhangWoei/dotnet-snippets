using CascadingConfiguration.Configuration.Diffing;

namespace CascadingConfiguration.Configuration.Child;

public sealed record ChildConfigurationDifference(FieldChange<string> Name, FieldChange<bool?> Enabled, FieldChange<bool?> Disabled);