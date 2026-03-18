namespace CascadingConfiguration;

public interface ICombiner<T>
{
    T Combine(T baseObject, T other);
}