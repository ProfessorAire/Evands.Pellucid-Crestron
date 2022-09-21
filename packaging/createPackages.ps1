param(
[Parameter(Mandatory=$true)][string]$libVersion,
[Parameter(Mandatory=$true)][string]$proVersion
)

$outPath = "$PSScriptRoot\.releases"

if ([System.IO.Directory]::Exists($outPath) -eq $false)
{
    $dir = [System.IO.Directory]::CreateDirectory($outPath)
    Write-Host "Created Directory: $dir"
}

Write-Host "Evands.Pellucid Version: $libVersion"
Write-Host "Evands.Pellucid.Pro Version: $proVersion"

Write-Host "Creating Nuget Package for Evands.Pellucid"
nuget pack $PSScriptRoot\Evands.Pellucid.nuspec -Version $libVersion -OutputDirectory $outPath -Verbosity quiet

$exitCode = 0

$libPath = "$outPath\Evands.Pellucid.$libVersion.nupkg"
$proPath = "$outPath\Evands.Pellucid.Pro.$proVersion.nupkg"

if ([System.IO.File]::Exists($libPath) -eq $false)
{
    Write-Warning "Unable to create nuget package for Evands.Pellucid, unable to find the file: $libPath"
    $exitCode = 1002
}
else
{
    Write-Host "Creating Nuget Package for Evands.Pellucid.Pro"
    (Get-Content -Path "$PSScriptRoot\Evands.Pellucid.Pro.nuspec") -Replace "depVer", $libVersion | Set-Content -Path "$PSScriptRoot\Evands.Pellucid.Pro.temp.nuspec"
    nuget pack $PSScriptRoot\Evands.Pellucid.Pro.temp.nuspec -Version $proVersion -OutputDirectory $outPath -Verbosity quiet
    if ([System.IO.File]::Exists($proPath) -eq $false)
    {
        Write-Warning "Unable to create nuget package for Evands.Pellucid.Pro"
        $exitCode = 1003
    }

    if ([System.IO.File]::Exists("$PSScriptRoot\Evands.Pellucid.Pro.temp.nuspec") -eq $true)
    {
        Remove-Item -Path "$PSScriptRoot\Evands.Pellucid.Pro.temp.nuspec"
    }
}

if ($exitCode -eq 0)
{
    $lib3Dll = "$PSScriptRoot/../src/Series3/Evands.Pellucid/bin/Release/Evands.Pellucid.dll"
    $lib3Xml = "$PSScriptRoot/../src/Series3/Evands.Pellucid/bin/Release/Evands.Pellucid.xml"
    $lib3ProDll = "$PSScriptRoot/../src/Series3/Evands.Pellucid.Pro/bin/Release/Evands.Pellucid.Pro.dll"
    $lib3ProXml = "$PSScriptRoot/../src/Series3/Evands.Pellucid.Pro/bin/Release/Evands.Pellucid.Pro.xml"
    $lib4Dll = "$PSScriptRoot/../src/Series3/Evands.Pellucid/bin/Release/Evands.Pellucid.dll"
    $lib4Xml = "$PSScriptRoot/../src/Series3/Evands.Pellucid/bin/Release/Evands.Pellucid.xml"
    $lib4ProDll = "$PSScriptRoot/../src/Series3/Evands.Pellucid.Pro/bin/Release/Evands.Pellucid.Pro.dll"
    $lib4ProXml = "$PSScriptRoot/../src/Series3/Evands.Pellucid.Pro/bin/Release/Evands.Pellucid.Pro.xml"
    $demo = "$PSScriptRoot/../src/Series3/Evands.Pellucid.ProDemo/bin/Release/Evands.Pellucid.ProDemo.cpz"
    
    $archivePath = "$outPath\Evands.Pellucid-Crestron-v$libVersion.zip"
    Write-Host "Preparing release archive."

    if ([System.IO.File]::Exists($archivePath) -eq $true)
    {
        Write-Host "Package already exists, deleting it now."
        Remove-Item $archivePath
    }

    try
    {
        $tempBuildPath = "$PSScriptRoot/.releases/packaging-$libVersion"
        Write-Host "Temp Build Path = $tempBuildPath"
        $temp3BuildPath = "$tempBuildPath/Series3"
        $temp4BuildPath = "$tempBuildPath/Series4"

        if ([System.IO.Directory]::Exists($tempBuildPath) -eq $false)
        {
            $dir = [System.IO.Directory]::CreateDirectory($tempBuildPath)
            Write-Host "Created Directory: $dir"
        }

        if ([System.IO.Directory]::Exists($temp3BuildPath) -eq $false)
        {
            $dir = [System.IO.Directory]::CreateDirectory($temp3BuildPath)
            Write-Host "Created Directory: $dir"
        }

        if ([System.IO.Directory]::Exists($temp4BuildPath) -eq $false)
        {
            $dir = [System.IO.Directory]::CreateDirectory($temp4BuildPath)
            Write-Host "Created Directory: $dir"
        }

        Write-Host "Copying Files"

        Copy-Item $lib3Dll -Destination $temp3BuildPath -Force
        Copy-Item $lib3Xml -Destination $temp3BuildPath -Force
        Copy-Item $lib3ProDll -Destination $temp3BuildPath -Force
        Copy-Item $lib3ProXml -Destination $temp3BuildPath -Force
        Copy-Item $lib4Dll -Destination $temp4BuildPath -Force
        Copy-Item $lib4Xml -Destination $temp4BuildPath -Force
        Copy-Item $lib4ProDll -Destination $temp4BuildPath -Force
        Copy-Item $lib4ProXml -Destination $temp4BuildPath -Force
        Copy-Item $demo -Destination $tempBuildPath -Force

        Write-Host "Compressing release archive."
        Get-ChildItem -Path $tempBuildPath | 
            Compress-Archive -DestinationPath $archivePath
            
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