namespace CascadingConfiguration.Configurations.Diffing;

public record FieldChange<T>(string FieldName, T? Old, T? New, ChangeType Type);