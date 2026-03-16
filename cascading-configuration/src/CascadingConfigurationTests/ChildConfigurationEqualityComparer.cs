using CascadingConfiguration;

namespace CascadingConfigurationTests;

internal sealed class ChildConfigurationEqualityComparer : IEqualityComparer<ChildConfiguration>
{
    public static ChildConfigurationEqualityComparer Instance = new();
    
    public bool Equals(ChildConfiguration? x, ChildConfiguration? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x is null || y is null)
        {
            return false;
        }
        
        return x.Enabled == y.Enabled 
               && x.Disabled == y.Disabled;
    }

    public int GetHashCode(ChildConfiguration obj)
    {
        return HashCode.Combine(obj.Enabled, obj.Disabled);
    }
}