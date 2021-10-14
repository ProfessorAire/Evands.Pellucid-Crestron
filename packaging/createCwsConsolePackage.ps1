param(
[Parameter(Mandatory=$true)][string]$version
)

$outPath = "$PSScriptRoot\.releases"

if ([System.IO.Directory]::Exists($outPath) -eq $false)
{
    $dir = [System.IO.Directory]::CreateDirectory($outPath)
    Write-Host "Created Directory: $dir"
}

Write-Host "Evands.Pellucid.CwsConsole Version: $version"

Write-Host "Creating Nuget Package for Evands.Pellucid.CwsConsole"
nuget pack $PSScriptRoot\Evands.Pellucid.CwsConsole.nuspec -Version $version -OutputDirectory $outPath -Verbosity quiet

$exitCode = 0

$cwsPath = "$outPath\Evands.Pellucid.CwsConsole.$version.nupkg"

if ([System.IO.File]::Exists($cwsPath) -eq $false)
{
    Write-Warning "Unable to create nuget package for Evands.Pellucid.CwsConsole"
    $exitCode = 1002
}

$cwsPath
exit $exitCode