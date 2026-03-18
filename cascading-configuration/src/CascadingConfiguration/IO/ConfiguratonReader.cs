using System.Text.Json;
using CascadingConfiguration.Configurations;
using CascadingConfiguration.IO.Models;

namespace CascadingConfiguration.IO;

public sealed class ConfiguratonReader
{
    public Configuration ReadFromString(string json)
    {
        var configurationData = ConfigurationDeserializer.Deserialize(json);
        return ConfigurationConverter.Convert(configurationData);
    }

    public async Task<Configuration> ReadFromFile(string filePath, CancellationToken cancellationToken)
    {
        await using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
        var configurationData = await ConfigurationDeserializer.Deserialize(stream, cancellationToken);

        return ConfigurationConverter.Convert(configurationData);
    }
}

internal static class ConfigurationDeserializer
{
    public static ConfigurationData Deserialize(string json)
    {
        return JsonSerializer.Deserialize<ConfigurationData>(json) ?? throw new JsonException("Deserializer returned null");
    }
    
    public static async Task<ConfigurationData> Deserialize(Stream stream, CancellationToken cancellationToken)
    {
        var result = await JsonSerializer.DeserializeAsync<ConfigurationData>(stream, JsonSerializerOptions.Default, cancellationToken);
        
        return result  ?? throw new JsonException("Deserializer returned null");
    }
}