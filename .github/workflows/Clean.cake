// Clean.cake - Clean artifacts and publish directories

#load "./Environment.cake"

var artifactsDir = GetArtifactsPath();
var publishDir = GetPublishPath();

Task("Default")
    .Does(() =>
{
    if (DirectoryExists(artifactsDir))
    {
        DeleteDirectory(artifactsDir, new DeleteDirectorySettings { Recursive = true, Force = true });
    }
    
    CreateDirectory(artifactsDir);
    CreateDirectory(publishDir);
    Information("Cleaned artifacts and publish directories.");
});

RunTarget(Argument("target", "Default"));
