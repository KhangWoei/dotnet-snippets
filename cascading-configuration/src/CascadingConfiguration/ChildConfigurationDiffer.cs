namespace CascadingConfiguration;

public class ChildConfigurationDiffer
{
    public ChildConfigurationDifference Difference(ChildConfiguration baseConfiguration, ChildConfiguration other)
    {
        var nameChange = FieldChangeCalculator<string>.Calculate(nameof(ChildConfiguration.Name), baseConfiguration.Name, other.Name);
        var enabledChange = FieldChangeCalculator<bool?>.Calculate(nameof(ChildConfiguration.Enabled), baseConfiguration.Enabled, other.Enabled);
        var disabledChange = FieldChangeCalculator<bool?>.Calculate(nameof(baseConfiguration.Disabled), baseConfiguration.Disabled,baseConfiguration.Disabled);
        return new ChildConfigurationDifference(nameChange, enabledChange, disabledChange);
    }
}