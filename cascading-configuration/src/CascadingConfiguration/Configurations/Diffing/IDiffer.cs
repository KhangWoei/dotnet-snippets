namespace CascadingConfiguration.Configurations.Diffing;

public interface IDiffer<in T, out TR>
{
    TR Difference(T diffBase, T other);
}