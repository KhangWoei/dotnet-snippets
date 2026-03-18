namespace CascadingConfiguration;

public record FieldChange<T>(string FieldName, T? Old, T? New, ChangeType Type);