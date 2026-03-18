using CascadingConfiguration.Configurations;
using CascadingConfiguration.Configurations.Child;
using CascadingConfiguration.IO.Models;

namespace CascadingConfiguration.IO;

internal static class ConfigurationConverter
{
    public static Configuration Convert(ConfigurationData configurationData)
    {
        return new Configuration(configurationData.Name ?? string.Empty, Convert(configurationData.Childs ?? []));
    }

    private static ChildConfiguration[] Convert(ChildConfigurationData[] children) =>
        children.Select(c =>
                new ChildConfiguration(c.Name ?? string.Empty, c.Enabled, c.Disabled))
            .ToArray();

    public static ConfigurationData Convert(Configuration configuration)
    {
        return new ConfigurationData
        {
            Name = configuration.Name,
            Childs = Convert(configuration.Childs),
        };
    }

    private static ChildConfigurationData[] Convert(ChildConfiguration[] children) =>
        children.Select(c =>
                new ChildConfigurationData
                {
                    Name = c.Name,
                    Enabled = c.Enabled,
                    Disabled = c.Disabled
                }
            )
            .ToArray();
}