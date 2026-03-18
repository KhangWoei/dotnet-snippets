using CascadingConfiguration.Configuration.Child;

namespace CascadingConfigurationTests.Configuration.Child;

internal sealed class ChildConfigurationEqualityComparer : IEqualityComparer<ChildConfiguration>
{
    public static ChildConfigurationEqualityComparer Instance = new();

    public bool Equals(ChildConfiguration? left, ChildConfiguration? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Name == right.Name
               && left.Enabled == right.Enabled
               && left.Disabled == right.Disabled;
    }

    public int GetHashCode(ChildConfiguration obj)
    {
        return HashCode.Combine(obj.Name, obj.Enabled, obj.Disabled);
    }
}