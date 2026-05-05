// DetermineVersion.cake - Determine next semantic version from conventional commits
#nullable enable
using System.Text.RegularExpressions;
using System.Globalization;

#load "./Environment.cake"

var artifactsDir = GetArtifactsPath();

string RunGit(string args)
{
    var settings = new ProcessSettings { Arguments = args, RedirectStandardOutput = true, Silent = true };
    var proc = StartAndReturnProcess("git", settings);
    var output = proc.GetStandardOutput();
    proc.WaitForExit();
    return string.Join("\n", output ?? new string[0]);
}

string? GetLatestSemVerTag()
{
    var outStr = RunGit("tag --list \"v*.*.*\" --sort=-v:refname");
    if (string.IsNullOrWhiteSpace(outStr)) return null;
    var first = outStr.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
    return first;
}

string? GetLatestProductionTag()
{
    var outStr = RunGit("tag --list \"v*.*.*\" --sort=-v:refname");
    if (string.IsNullOrWhiteSpace(outStr)) return null;
    
    // Filter out pre-release tags (containing '-') to get only production releases
    var tags = outStr.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
    var productionTag = tags.FirstOrDefault(t => !t.Contains('-'));
    
    return productionTag;
}

int GetLatestBetaNumber(string baseVersionString)
{
    // Get all tags matching the base version with beta suffix
    var outStr = RunGit($"tag --list \"v{baseVersionString}-beta.*\" --sort=-v:refname");
    if (string.IsNullOrWhiteSpace(outStr)) return 0;
    
    var tags = outStr.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
    foreach (var tag in tags)
    {
        // Extract beta number from tag like "v1.0.0-beta.5"
        var match = Regex.Match(tag, @"-beta\.(\d+)$");
        if (match.Success && int.TryParse(match.Groups[1].Value, out var betaNum))
        {
            return betaNum;
        }
    }
    
    return 0;
}

IList<(string Sha, string Header, string Body)> GetCommitsSince(string? fromTag)
{
    string range = string.IsNullOrWhiteSpace(fromTag) ? "HEAD" : $"{fromTag}..HEAD";
    var fmt = "%H%x01%s%x01%b%x01==END==";
    var raw = RunGit($"log {range} --pretty=format:{fmt}");
    var entries = new List<(string, string, string)>();
    if (string.IsNullOrWhiteSpace(raw)) return entries;
    var parts = raw.Split(new[] { "==END==" }, StringSplitOptions.RemoveEmptyEntries);
    foreach (var p in parts)
    {
        var segs = p.Split('\x01');
        if (segs.Length >= 3)
        {
            var sha = segs[0].Trim();
            var header = segs[1].Trim();
            var body = string.Join("\n", segs.Skip(2)).Trim();
            entries.Add((sha, header, body));
        }
    }
    return entries;
}

bool IsBreaking((string Sha, string Header, string Body) commit)
{
    var header = commit.Header;
    var body = commit.Body ?? string.Empty;
    var m = Regex.Match(header, @"^[a-z]+(\([^)]+\))?(?<bang>!)?:", RegexOptions.IgnoreCase);
    if (m.Success && m.Groups["bang"].Success && m.Groups["bang"].Value == "!") return true;
    if (Regex.IsMatch(body, @"BREAKING CHANGE", RegexOptions.IgnoreCase)) return true;
    return false;
}

string? GetType((string Sha, string Header, string Body) commit)
{
    var m = Regex.Match(commit.Header, @"^(?<type>[a-z]+)(?:\([^\)]+\))?(?:!?)?:", RegexOptions.IgnoreCase);
    return m.Success ? m.Groups["type"].Value.ToLowerInvariant() : null;
}

SemanticVersion ParseSemVerTag(string tag)
{
    var v = tag.TrimStart('v', 'V');
    var parts = v.Split('-', 2);
    var numbers = parts[0].Split('.');
    int major = numbers.Length > 0 ? int.Parse(numbers[0], CultureInfo.InvariantCulture) : 0;
    int minor = numbers.Length > 1 ? int.Parse(numbers[1], CultureInfo.InvariantCulture) : 0;
    int patch = numbers.Length > 2 ? int.Parse(numbers[2], CultureInfo.InvariantCulture) : 0;
    return new SemanticVersion(major, minor, patch);
}

SemanticVersion Bump(SemanticVersion baseVersion, string level)
{
    if (level == "major") return new SemanticVersion(baseVersion.Major + 1, 0, 0);
    if (level == "minor") return new SemanticVersion(baseVersion.Major, baseVersion.Minor + 1, 0);
    return new SemanticVersion(baseVersion.Major, baseVersion.Minor, baseVersion.Patch + 1);
}

Task("Default")
    .Does(() =>
{
    if (!DirectoryExists(artifactsDir)) CreateDirectory(artifactsDir);

    // Get current branch name
    var currentBranch = RunGit("rev-parse --abbrev-ref HEAD").Trim();
    var isMainBranch = currentBranch == "main" || currentBranch == "master";
    
    // Check for GITHUB_REF environment variable (GitHub Actions)
    var githubRef = EnvironmentVariable("GITHUB_REF");
    if (!string.IsNullOrEmpty(githubRef))
    {
        isMainBranch = githubRef == "refs/heads/main" || githubRef == "refs/heads/master";
        currentBranch = githubRef.Replace("refs/heads/", "");
    }
    
    Information($"Current branch: {currentBranch}, Is main: {isMainBranch}");

    var latestTag = GetLatestSemVerTag();
    Information($"Latest tag: {latestTag ?? "(none)"}");
    
    // For changelog on main branch, use latest production tag
    // For pre-releases, use the actual latest tag
    var changelogBaseTag = latestTag;
    if (isMainBranch)
    {
        changelogBaseTag = GetLatestProductionTag();
    }
    
    Information($"Changelog base tag: {changelogBaseTag ?? "(none)"}");
    
    // Check if latest tag is a pre-release
    bool latestIsPrerelease = latestTag != null && latestTag.Contains("-beta.");
    SemanticVersion baseVersion;
    
    if (latestTag is null)
    {
        baseVersion = new SemanticVersion(1, 0, 0);
    }
    else
    {
        baseVersion = ParseSemVerTag(latestTag);
    }

    var commits = GetCommitsSince(changelogBaseTag);
    if (commits.Count == 0)
    {
        throw new Exception("No changes to release. If you need to republish a version, please delete the previous tag and rerun the workflow.");
    }

    bool anyBreaking = false;
    bool anyFeat = false;
    bool anyFix = false;
    var breakingChanges = new List<(string ShortSha, string Header, string Body)>();
    var grouped = new Dictionary<string, List<(string ShortSha, string Header, string Body)>>();

    // Allowed types for changelog (unless breaking)
    var allowedTypes = new HashSet<string> { "feat", "fix", "docs", "task" };

    foreach (var c in commits)
    {
        var isBreaking = IsBreaking(c);
        var type = GetType(c) ?? "other";

        if (isBreaking)
        {
            anyBreaking = true;
            breakingChanges.Add((c.Sha.Substring(0, 7), c.Header, c.Body));
        }

        if (type == "feat") anyFeat = true;
        if (type == "fix") anyFix = true;

        // Only group non-breaking changes of allowed types
        if (!isBreaking && allowedTypes.Contains(type))
        {
            if (!grouped.ContainsKey(type)) grouped[type] = new List<(string, string, string)>();
            grouped[type].Add((c.Sha.Substring(0, 7), c.Header, c.Body));
        }
    }

    SemanticVersion newVersion;
    string currentBumpLevel = anyBreaking ? "major" : anyFeat ? "minor" : "patch";
    
    if (latestTag is null)
    {
        Information("No existing version tag found. Assuming base version {0}.", baseVersion);
        newVersion = baseVersion;
    }
    else if (isMainBranch || !latestIsPrerelease)
    {
        // On main branch or if previous was not a prerelease, always bump version
        newVersion = Bump(baseVersion, currentBumpLevel);
        Information($"New version: {newVersion} (level: {currentBumpLevel})");
    }
    else
    {
        // Latest tag is a pre-release, we need to check what level the previous tag was at
        var allTags = RunGit("tag --list \"v*.*.*\" --sort=-v:refname").Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        
        // Find the production tag that the current pre-release is based on
        string? baseProductionTag = null;
        foreach (var tag in allTags)
        {
            if (!tag.Contains("-beta."))
            {
                var tagVersion = ParseSemVerTag(tag);
                if (tagVersion.Major < baseVersion.Major ||
                    (tagVersion.Major == baseVersion.Major && tagVersion.Minor < baseVersion.Minor) ||
                    (tagVersion.Major == baseVersion.Major && tagVersion.Minor == baseVersion.Minor && tagVersion.Patch < baseVersion.Patch))
                {
                    baseProductionTag = tag;
                    break;
                }
            }
        }
        
        // Get all commits from the base production tag to determine previous bump level
        string? previousBumpLevel = null;
        if (baseProductionTag != null)
        {
            var preReleaseCommits = GetCommitsSince(baseProductionTag);
            bool hadBreaking = false;
            bool hadFeat = false;
            
            foreach (var c in preReleaseCommits)
            {
                if (IsBreaking(c)) { hadBreaking = true; break; }
                var type = GetType(c) ?? "other";
                if (type == "feat") hadFeat = true;
            }
            
            previousBumpLevel = hadBreaking ? "major" : hadFeat ? "minor" : "patch";
            Information($"Previous pre-release was created with bump level: {previousBumpLevel}");
        }
        
        bool shouldBumpVersion = false;
        
        if (previousBumpLevel == null)
        {
            shouldBumpVersion = true;
        }
        else if (currentBumpLevel == "major" && previousBumpLevel != "major")
        {
            shouldBumpVersion = true;
        }
        else if (currentBumpLevel == "minor" && previousBumpLevel == "patch")
        {
            shouldBumpVersion = true;
        }
        
        if (shouldBumpVersion)
        {
            newVersion = Bump(baseVersion, currentBumpLevel);
            Information($"New version: {newVersion} (level: {currentBumpLevel}, requires version bump from previous {previousBumpLevel})");
        }
        else
        {
            newVersion = baseVersion;
            Information($"Bump level '{currentBumpLevel}' does not exceed previous '{previousBumpLevel}', incrementing beta number only.");
        }
    }
    
    // Add pre-release suffix if not on main branch
    string versionString;
    if (!isMainBranch)
    {
        var latestBetaNumber = GetLatestBetaNumber(newVersion.ToString());
        var betaNumber = latestBetaNumber + 1;
        
        versionString = $"{newVersion}-beta.{betaNumber}";
        Information($"Pre-release version: {versionString}");
        SetEnvironmentVariable("IS_PRERELEASE", "true");
    }
    else
    {
        versionString = newVersion.ToString();
        SetEnvironmentVariable("IS_PRERELEASE", "false");
    }

    var changelog = new System.Text.StringBuilder();
    changelog.AppendLine($"# Release v{versionString} - {DateTime.UtcNow:yyyy-MM-dd}");
    changelog.AppendLine();

    // Breaking Changes section (if any)
    if (breakingChanges.Count > 0)
    {
        changelog.AppendLine("## Breaking Changes");
        changelog.AppendLine();
        foreach (var entry in breakingChanges)
        {
            var stripped = Regex.Replace(entry.Header, @"^[a-z]+(\([^\)]+\))?(?:!?)?:\s*", "", RegexOptions.IgnoreCase);
            changelog.AppendLine($"* {stripped} ({entry.ShortSha})");
        }
        changelog.AppendLine();
    }

    // Order sections: feat, fix, docs, task
    var count = 0;
    var orderedTypes = new[] { "feat", "fix", "docs", "task" };
    foreach (var type in orderedTypes)
    {
        if (!grouped.ContainsKey(type)) continue;
        count++;
        var heading = type switch
        {
            "feat" => "## Features",
            "fix" => "## Bug Fixes",
            "docs" => "## Documentation",
            "task" => "## Other Tasks",
            _ => $"## {type}"
        };
        changelog.AppendLine(heading);
        changelog.AppendLine();
        foreach (var entry in grouped[type])
        {
            var stripped = Regex.Replace(entry.Header, @"^[a-z]+(\([^\)]+\))?(?:!?)?:\s*", "", RegexOptions.IgnoreCase);
            changelog.AppendLine($"* {stripped} ({entry.ShortSha})");
        }
        changelog.AppendLine();
    }

    if (count == 0 && breakingChanges.Count == 0 && newVersion.ToString() == "1.0.0")
    {
        changelog.AppendLine("* Initial Release");
    }
    else if (count == 0 && breakingChanges.Count == 0)
    {
        throw new Exception("No commits found for changelog. Please ensure commits follow Conventional Commits specification.");
    }

    SetEnvironmentVariable("RELEASE_VERSION", versionString);
    System.IO.File.WriteAllText(System.IO.Path.Combine(artifactsDir, "changelog.md"), changelog.ToString());
    Information($"Wrote RELEASE_VERSION={versionString} and changelog.md");
});

RunTarget(Argument("target", "Default"));
record SemanticVersion(int Major, int Minor, int Patch)
{
    public override string ToString() => $"{Major}.{Minor}.{Patch}";
}
