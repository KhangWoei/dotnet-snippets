using CascadingConfiguration;

namespace CascadingConfigurationTests;

internal sealed class ConfigurationEqualityComparer : IEqualityComparer<Configuration>
{
    public static ConfigurationEqualityComparer Instance = new ();
    
    public bool Equals(Configuration? left, Configuration? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Name == right.Name;
    }

    public int GetHashCode(Configuration configuration)
    {
        return HashCode.Combine(configuration.Name.GetHashCode());
    }
}
