namespace CascadingConfiguration;

public sealed record ChildConfigurationDifference(FieldChange<string> Name, FieldChange<bool?> Enabled, FieldChange<bool?> Disabled);