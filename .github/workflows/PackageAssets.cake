// PackageAssets.cake - Package build outputs into a release zip
#load "./Environment.cake"
var artifactsDir = GetArtifactsPath();

Task("Default")
    .Does(() =>
{
    var derivedVersion = GetEnvironmentVariable("RELEASE_VERSION");

    if (derivedVersion is null) throw new InvalidOperationException("Derived version not found. Run DetermineVersion first.");

    // Check for required build outputs
    var libDll = System.IO.Path.GetFullPath("../../src/Evands.Pellucid/bin/Release/netstandard2.0/Evands.Pellucid.dll");
    var libXml = System.IO.Path.GetFullPath("../../src/Evands.Pellucid/bin/Release/netstandard2.0/Evands.Pellucid.xml");
    var demoCpz = System.IO.Path.GetFullPath("../../src/Evands.Pellucid.ProDemo/bin/Release/net8.0/Evands.Pellucid.ProDemo.cpz");

    if (!System.IO.File.Exists(libDll)) throw new InvalidOperationException($"Library DLL not found at {libDll}. Run build first.");
    if (!System.IO.File.Exists(libXml)) throw new InvalidOperationException($"Library XML not found at {libXml}. Run build first.");

    var changelogPath = System.IO.Path.Combine(artifactsDir, "changelog.md");
    if (!System.IO.File.Exists(changelogPath)) throw new InvalidOperationException("changelog.md not found in artifacts directory.");

    var tmpFolder = System.IO.Path.Combine(artifactsDir, $"Evands.Pellucid-Crestron-{derivedVersion}");
    if (DirectoryExists(tmpFolder)) DeleteDirectory(tmpFolder, new DeleteDirectorySettings { Recursive = true, Force = true });
    CreateDirectory(tmpFolder);

    CopyFile(libDll, System.IO.Path.Combine(tmpFolder, "Evands.Pellucid.dll"));
    CopyFile(libXml, System.IO.Path.Combine(tmpFolder, "Evands.Pellucid.xml"));
    CopyFile(changelogPath, System.IO.Path.Combine(tmpFolder, "changelog.md"));

    if (System.IO.File.Exists(demoCpz))
    {
        CopyFile(demoCpz, System.IO.Path.Combine(tmpFolder, "Evands.Pellucid.ProDemo.cpz"));
    }
    else
    {
        Warning("Demo CPZ not found; skipping. This is expected if the SDK.Program package is not available.");
    }

    var zipName = $"Evands.Pellucid-Crestron-v{derivedVersion}.zip";
    var zipPath = System.IO.Path.Combine(artifactsDir, zipName);
    Zip(tmpFolder, zipPath);
    Information($"Packaged {zipPath}");
});

RunTarget(Argument("target", "Default"));
