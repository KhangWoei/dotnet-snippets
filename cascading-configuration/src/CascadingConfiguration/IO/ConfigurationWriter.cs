using System.Text.Json;
using System.Text.Json.Serialization;
using CascadingConfiguration.Configurations;
using CascadingConfiguration.IO.Models;

namespace CascadingConfiguration.IO;

public sealed class ConfigurationWriter
{
    public async Task<string> WriteToString(Configuration configuration, CancellationToken cancellationToken)
    {
        var configurationData = ConfigurationConverter.Convert(configuration);
        await using var stream = new MemoryStream();

        await ConfigurationSerializer.Serialize(stream, configurationData, null, cancellationToken);

        await stream.FlushAsync(cancellationToken);
        stream.Seek(0, SeekOrigin.Begin);

        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync(cancellationToken);
    }

    public async Task WriteToFile(Configuration configuration, string filePath, CancellationToken cancellationToken)
    {
        var configurationData = ConfigurationConverter.Convert(configuration);
        await using var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
        await ConfigurationSerializer.Serialize(stream, configurationData, null, cancellationToken);
    }
}

internal static class ConfigurationSerializer
{
    private static readonly JsonSerializerOptions Options = new() {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };
    
    public static async Task Serialize(Stream stream, ConfigurationData configuration, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
    {
        await JsonSerializer.SerializeAsync(stream, configuration, options ?? Options, cancellationToken);
    }
}