// CreateGitHubRelease.cake - Create GitHub release with tag and changelog

using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
#load "./Environment.cake"
var artifactsDir = GetArtifactsPath();

string ReadArtifactFile(string fileName)
{
    var path = System.IO.Path.Combine(artifactsDir, fileName);
    if (!System.IO.File.Exists(path)) throw new InvalidOperationException($"Required artifact '{fileName}' not found. Run the appropriate upstream task first.");
    return System.IO.File.ReadAllText(path);
}

Task("Default")
    .Does(async () =>
{
    var githubToken = Argument("githubToken", EnvironmentVariable("GITHUB_TOKEN"));
    if (string.IsNullOrWhiteSpace(githubToken)) throw new InvalidOperationException("GitHub token not provided. Pass --githubToken or set GITHUB_TOKEN env var.");

    var owner = Argument("repoOwner", "");
    var repo = Argument("repoName", "");
    var repoEnv = EnvironmentVariable("GITHUB_REPOSITORY");
    if (string.IsNullOrWhiteSpace(repoEnv))
    {
        if (!string.IsNullOrWhiteSpace(owner) && !string.IsNullOrWhiteSpace(repo))
        {
            Information("Using provided repoOwner and repoName arguments for local run.");
        }
        else
        {
            throw new InvalidOperationException("GITHUB_REPOSITORY environment variable not found. Local runs should pass --repoOwner and --repoName instead.");
        }
    }
    else
    {
        var repoParts = repoEnv.Split('/');
        owner = repoParts[0];
        repo = repoParts[1];
    }

    var version = GetEnvironmentVariable("RELEASE_VERSION");
    if (string.IsNullOrWhiteSpace(version))
    {
        throw new InvalidOperationException("RELEASE_VERSION environment variable not found. Run DetermineVersion task first.");
    }
    
    var isPrerelease = GetEnvironmentVariable("IS_PRERELEASE") == "true";
    Information($"Creating release v{version} (prerelease: {isPrerelease})");

    var changelog = ReadArtifactFile("changelog.md");
    var targetCommitish = Argument("targetCommitish", EnvironmentVariable("GITHUB_SHA") ?? "main");

    var payload = new {
        tag_name = $"v{version}",
        name = $"v{version}",
        body = changelog,
        draft = true,
        prerelease = isPrerelease,
        target_commitish = targetCommitish
    };

    using var http = new System.Net.Http.HttpClient();
    http.DefaultRequestHeaders.UserAgent.ParseAdd("Evands.Pellucid-Cake-Release");
    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", githubToken);

    var url = $"https://api.github.com/repos/{owner}/{repo}/releases";
    var content = new StringContent(JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json");
    var resp = await http.PostAsync(url, content);
    var body = await resp.Content.ReadAsStringAsync();
    if (!resp.IsSuccessStatusCode) throw new InvalidOperationException($"Failed to create release: {resp.StatusCode}\n{body}");

    using var doc = JsonDocument.Parse(body);
    var uploadUrlTemplate = doc.RootElement.GetProperty("upload_url").GetString();
    var releaseId = doc.RootElement.GetProperty("id").GetInt32();
    Information($"Created release id {releaseId}");

    SetEnvironmentVariable("UPLOAD_URL", uploadUrlTemplate ?? "");
});

RunTarget(Argument("target", "Default"));
