param(
[Parameter(Mandatory=$true)][string]$libVersion
)

$outPath = "$PSScriptRoot/.releases"

if ([System.IO.Directory]::Exists($outPath) -eq $false)
{
    $dir = [System.IO.Directory]::CreateDirectory($outPath)
    Write-Host "Created Directory: $dir"
}

Write-Host "Evands.Pellucid Version: $libVersion"

Write-Host "Creating Nuget Package for Evands.Pellucid"
dotnet pack "$PSScriptRoot/../src/Evands.Pellucid/Evands.Pellucid.csproj" --configuration Release -p:PackageVersion=$libVersion --output $outPath

$exitCode = 0

$libPath = "$outPath/Evands.Pellucid.$libVersion.nupkg"

if ([System.IO.File]::Exists($libPath) -eq $false)
{
    Write-Warning "Unable to create nuget package for Evands.Pellucid"
    $exitCode = 1002
}

if ($exitCode -eq 0)
{
    $libDll = "$PSScriptRoot/../src/Evands.Pellucid/bin/Release/netstandard2.0/Evands.Pellucid.dll"
    $libXml = "$PSScriptRoot/../src/Evands.Pellucid/bin/Release/netstandard2.0/Evands.Pellucid.xml"
    
    $archivePath = "$outPath/Evands.Pellucid-Crestron-v$libVersion.zip"
    Write-Host "Creating release archive."

    if ([System.IO.File]::Exists($archivePath) -eq $true)
    {
        Write-Host "Package already exists, deleting it now."
        Remove-Item $archivePath
    }

    try
    {
        Compress-Archive $libDll, $libXml -DestinationPath $archivePath
        Write-Host "Created release archive '$archivePath'."
    }
    catch
    {
    }

    if ([System.IO.File]::Exists($archivePath) -eq $false)
    {
        Write-Host "Error creating the release archive."
        $exitCode = 1004
    }
}

if ($exitCode -eq 0)
{
    $libPath, $archivePath
}

exit $exitCode