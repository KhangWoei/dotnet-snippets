namespace CascadingConfiguration.Configurations.Diffing;

internal static class FieldChangeCalculator<T>
{
    public static FieldChange<T> Calculate(string name, T? oldValue, T? newValue)
    {
        var normalizedOld = Normalize(oldValue);
        var normalizedNew = Normalize(newValue);
        
        var changeType = EqualityComparer<T>.Default.Equals(normalizedOld, normalizedNew)
            ? ChangeType.Unchanged
            : normalizedOld is null
                ? ChangeType.Added
                : normalizedNew is null
                    ? ChangeType.Deleted
                    : ChangeType.Updated;

        return new FieldChange<T>(name, oldValue, newValue, changeType);

        T? Normalize(T? input)
        {
            if (typeof(T) == typeof(string) && string.IsNullOrEmpty(input as string))
            {
                return default;
            }

            return input;
        } 
    }
}