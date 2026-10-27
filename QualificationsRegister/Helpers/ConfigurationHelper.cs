using Microsoft.Extensions.Configuration;

namespace Ofqual.Common.RegisterAPI.Helpers;

public static class ConfigurationHelper
{
    private static IConfiguration? _config = null;

    public static void Initialise(IConfiguration config)
    {
        _config = config;
    }

    private static IConfiguration GetConfiguration()
    {
        return _config ??
            throw new InvalidOperationException(
                "ConfigurationHelper has not been initialised. " +
                "ConfigurationHelper.Initialise() must be called first.");
    }

    public static string? GetValue(string key)
    {
        return GetConfiguration().GetValue<string>(key)
            ?? Environment.GetEnvironmentVariable(key);
    }
}