namespace CascadingConfiguration.Configurations.Child;

public sealed class ChildConfigurationCollectionDifference(
    ICollection<ChildConfiguration> added,
    ICollection<ChildConfiguration> deleted,
    ICollection<ChildConfigurationDifference> updates)
{
    public ICollection<ChildConfiguration> Added { get; } = added;
    
    public ICollection<ChildConfiguration> Deleted { get; } = deleted;
    
    public ICollection<ChildConfigurationDifference> Updated { get; } = updates;
}