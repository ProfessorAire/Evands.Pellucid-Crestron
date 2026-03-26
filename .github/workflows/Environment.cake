// Environment.cake - Utilities for setting and retrieving typed environment variables
#nullable enable

using System.Globalization;
using System.Text.Json;

// Environment variables cache
Dictionary<string, string>? _environmentVariables = null;
string? _workingPath = null;

// Get the working path for environment variables
string GetWorkingPath()
{
    if (_workingPath != null) return _workingPath;
    
    // Check if running on GitHub Actions
    var githubEnv = Environment.GetEnvironmentVariable("GITHUB_ENV");
    if (!string.IsNullOrEmpty(githubEnv))
    {
        _workingPath = githubEnv;
        Information($"Using GitHub environment file: {_workingPath}");
    }
    else
    {
        _workingPath = System.IO.Path.GetFullPath("../../artifacts/.cake_env_vars");
        Information($"Using local environment file: {_workingPath}");
    }
    
    return _workingPath;
}

// Load environment variables from file
Dictionary<string, string> LoadEnvironmentVariables()
{
    if (_environmentVariables != null) return _environmentVariables;
    
    _environmentVariables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    var path = GetWorkingPath();
    
    // On GitHub Actions, check if the file exists and load it
    if (System.IO.File.Exists(path))
    {
        foreach (var line in System.IO.File.ReadAllLines(path))
        {
            var idx = line.IndexOf('=');
            if (idx > 0)
            {
                var key = line.Substring(0, idx);
                var val = line.Substring(idx + 1);
                _environmentVariables[key] = val;
            }
        }
    }
    
    return _environmentVariables;
}

// Save environment variables to file
void SaveEnvironmentVariables()
{
    if (_environmentVariables == null) return;
    
    var path = GetWorkingPath();
    var lines = _environmentVariables.Select(kvp => $"{kvp.Key}={kvp.Value}");
    System.IO.File.WriteAllLines(path, lines);
}

// Set a string environment variable
void SetEnvironmentVariable(string name, string value)
{
    var vars = LoadEnvironmentVariables();
    vars[name] = value;
    SaveEnvironmentVariables();
    Information($"Set environment variable: {name}={value}");
}

// Set a strongly-typed environment variable using JSON serialization
void SetEnvironmentVariable<T>(string name, T value)
{
    var json = JsonSerializer.Serialize(value);
    SetEnvironmentVariable(name, json);
}

// Get a string environment variable
string? GetEnvironmentVariable(string name)
{
    // On GitHub Actions, check system environment first (from previous steps)
    var systemValue = Environment.GetEnvironmentVariable(name);
    if (!string.IsNullOrEmpty(systemValue)) return systemValue;
    
    // Fall back to our cached file values
    var vars = LoadEnvironmentVariables();
    return vars.TryGetValue(name, out var value) ? value : null;
}

// Get a strongly-typed environment variable using JSON deserialization
T? GetEnvironmentVariable<T>(string name)
{
    var value = GetEnvironmentVariable(name);
    if (string.IsNullOrEmpty(value)) return default;
    
    try
    {
        return JsonSerializer.Deserialize<T>(value);
    }
    catch
    {
        Warning($"Failed to deserialize environment variable '{name}' as type {typeof(T).Name}");
        return default;
    }
}

// Get a strongly-typed environment variable with a default value
T GetEnvironmentVariable<T>(string name, Func<T> defaultValueFactory)
{
    var result = GetEnvironmentVariable<T>(name);
    return result is null ? defaultValueFactory() : result;
}

string GetArtifactsPath()
{
    return GetEnvironmentVariable("ARTIFACTS_PATH", () => System.IO.Path.GetFullPath("../../artifacts"));
}

string GetPublishPath()
{
    return GetEnvironmentVariable("PUBLISH_PATH", () => System.IO.Path.Combine(GetArtifactsPath(), "publish"));
}
