using Microsoft.Extensions.Configuration;

namespace AgenticAI;

public static class Utils
{
    private static IConfigurationRoot? _configuration;
    public static IConfigurationRoot Configuration
    {
        get
        {
            if (_configuration == null)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
                _configuration = builder.Build();
            }

            return _configuration;
        }
    }

    public static string GetConfigurationValue(string key)
    {
        return Configuration[key] ??
            throw new KeyNotFoundException($"Configuration key '{key}' not found.");
    }
}