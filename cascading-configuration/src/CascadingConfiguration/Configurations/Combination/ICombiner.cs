namespace CascadingConfiguration.Configurations.Combination;

public interface ICombiner<T>
{
    T Combine(T baseObject, T other);
}