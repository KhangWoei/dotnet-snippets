namespace CascadingConfiguration.Configuration.Combination;

public interface ICombiner<T>
{
    T Combine(T baseObject, T other);
}