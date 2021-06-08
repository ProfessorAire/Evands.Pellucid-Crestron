param(
[Parameter(Mandatory=$true)][string]$libVersion,
[Parameter(Mandatory=$true)][string]$proVersion
)

$outPath = "$PSScriptRoot/.releases"

if ([System.IO.Directory]::Exists($outPath) -eq $false)
{
    [System.IO.Directory]::CreateDirectory($outPath)
}

Write-Host "Evands.Pellucid Version: $libVersion"
Write-Host "Evands.Pellucid.Pro Version: $proVersion"

Write-Host "Creating Nuget Package for Evands.Pellucid"
nuget pack $PSScriptRoot\Evands.Pellucid.nuspec -Version $libVersion -OutputDirectory $outPath -Verbosity quiet

$exitCode = 0

$libPath = "$outPath/Evands.Pellucid.$libVersion.nupkg"
$proPath = "$outPath/Evands.Pellucid.Pro.$proVersion.nupkg"

if ([System.IO.File]::Exists($libPath) -eq $false)
{
    Write-Warning "Unable to create nuget package for Evands.Pellucid"
    $exitCode = 1002
}
else
{
    Write-Host "Creating Nuget Package for Evands.Pellucid.Pro"
    (Get-Content -Path "$PSScriptRoot/Evands.Pellucid.Pro.nuspec") -Replace "depVer", $libVersion | Set-Content -Path "$PSScriptRoot/Evands.Pellucid.Pro.temp.nuspec"
    nuget pack $PSScriptRoot/Evands.Pellucid.Pro.temp.nuspec -Version $proVersion -OutputDirectory $outPath -Verbosity quiet
    if ([System.IO.File]::Exists($proPath) -eq $false)
    {
        Write-Warning "Unable to create nuget package for Evands.Pellucid.Pro"
        $exitCode = 1003
    }

    if ([System.IO.File]::Exists("$PSScriptRoot/Evands.Pellucid.Pro.temp.nuspec") -eq $true)
    {
        Remove-Item -Path "$PSScriptRoot/Evands.Pellucid.Pro.temp.nuspec"
    }
}

if ($exitCode -eq 0)
{
    $libDll = "$PSScriptRoot/../src/Evands.Pellucid/bin/Release/Evands.Pellucid.dll"
    $libXml = "$PSScriptRoot/../src/Evands.Pellucid/bin/Release/Evands.Pellucid.xml"
    $libProDll = "$PSScriptRoot/../src/Evands.Pellucid.Pro/bin/Release/Evands.Pellucid.Pro.dll"
    $libProXml = "$PSScriptRoot/../src/Evands.Pellucid.Pro/bin/Release/Evands.Pellucid.Pro.xml"
    $demo = "$PSScriptRoot/../src/Evands.Pellucid.ProDemo/bin/Release/Evands.Pellucid.ProDemo.cpz"
    
    $archivePath = "$outPath/Evands.Pellucid-Crestron-v$libVersion.zip"
    Write-Host "Creating release archive."

    if ([System.IO.File]::Exists($archivePath) -eq $true)
    {
        Write-Host "Package already exists, deleting it now."
        Remove-Item $archivePath
    }

    try
    {
        Compress-Archive $libDll, $libXml, $libProDll, $libProXml, $demo -DestinationPath $archivePath
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
    $libPath, $proPath, $archivePath
}

exit $exitCode