function Get-LibVersion
{
    param(
        [string]$version
    )

    $parts = $version.Split(".")
    $part0 = $parts[0]
    $part1 = $parts[1]
    $part2 = $parts[2]
    $part3 = [int]$parts[3].Trim("`"")

    $lastPart = ""

    if ($part3 -ge 100 -and $part3 -lt 200)
    {
        $lastPart = "Alpha." + ($part3 - 99).ToString()
    }
    elseif ($part3 -ge 200 -and $part3 -lt 300)
    {
        $lastPart = "Beta." + ($part3 - 199).ToString()
    }
    elseif ($part3 -ge 300 -and $part3 -lt 1000)
    {
        $lastPart = "RC." + ($part3 - 299).ToString()
    }

    [string]$ver = "$part0.$part1.$part2"

    if ($lastPart -ne "")
    {
        $ver = "$ver-$lastPart"
    }

    return [string]$ver.Replace("`"", "")
}

$libContents = Get-Content -Path "$PSScriptRoot/../src/Evands.Pellucid/Evands.Pellucid.csproj"
$libProContents = Get-Content -Path "$PSScriptRoot/../src/Evands.Pellucid.Pro/Evands.Pellucid.Pro.csproj"
$libProDemoContents = Get-Content -Path "$PSScriptRoot/../src/Evands.Pellucid.ProDemo/Evands.Pellucid.ProDemo.csproj"

Write-Host "Creating new Visual Studio DTE Object"
$dte = New-Object -ComObject VisualStudio.DTE.9.0

$path = Split-Path $PSScriptRoot -Parent
$path = "$path\src\Evands.Pellucid.sln"

Write-Host "Attempting to open $path"
$dte.Solution.Open($path)

Write-Host "Cleaning the solution."
$dte.Solution.SolutionBuild.Clean($true)

$dte.Solution.SolutionBuild.SolutionConfigurations["Release"].Activate()

Write-Host "Active Configuration is " + $dte.Solution.SolutionBuild.ActiveConfiguration.Name
if ($dte.Solution.SolutionBuild.ActiveConfiguration.Name -eq "Release")
{
    Write-Host "Building Project"
    $dte.Solution.SolutionBuild.Build($true)
    $failed = $dte.Solution.SolutionBuild.LastBuildInfo

    "$failed projects failed to build."

    if ($failed -gt 0)
    {
        Write-Error -Message "Failed to build project." -Category InvalidResult -ErrorId 1000
        $exitCode = 1000
        return
    }
}

$libVersion = ""
$proVersion = ""

foreach ($proj in $dte.Solution.Projects)
{
    foreach ($rootItem in $proj.ProjectItems)
    {
        if ($rootItem.Name -eq "Properties")
        {
            foreach ($subItem in $rootItem.ProjectItems)
            {
                if ($subItem.Name -eq "AssemblyInfo.cs")
                {
                    foreach ($element in $subItem.FileCodeModel.CodeElements)
                    {
                        if ($element.Kind -eq 7)
                        {
                            $name = $element.Name
                            $ver = $element.Value

                            if ($name -eq "AssemblyVersion")
                            {
                                if ($proj.Name -eq "Evands.Pellucid")
                                {
                                    $libVersion = $ver
                                }
                                elseif ($proj.Name -eq "Evands.Pellucid.Pro")
                                {
                                    $proVersion = $ver
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

$dte.Solution.Close($false)
$dte.Quit()

Set-Content -Path "$PSScriptRoot/../src/Evands.Pellucid/Evands.Pellucid.csproj" -Value $libContents
Set-Content -Path "$PSScriptRoot/../src/Evands.Pellucid.Pro/Evands.Pellucid.Pro.csproj" -Value $libProContents
Set-Content -Path "$PSScriptRoot/../src/Evands.Pellucid.ProDemo/Evands.Pellucid.ProDemo.csproj" -Value $libProDemoContents

$exitCode = 0

if ($libVersion -eq "" -or $proVersion -eq "")
{
    Write-Host "Unable to find version information, cannot create packages."
    $exitCode = 1001
}
else
{
    $ver1 = Get-LibVersion $libVersion
    $ver2 = Get-LibVersion $proVersion
    
    $outPath = "$PSScriptRoot/.releases"

    if ([System.IO.Directory]::Exists($outPath) -eq $false)
    {
        [System.IO.Directory]::CreateDirectory($outPath)
    }

    Write-Host "Final Evands.Pellucid Version: $ver1"
    Write-Host "Final Evands.Pellucid.Pro Version: $ver2"

    Write-Host "Creating Nuget Package for Evands.Pellucid"
    nuget pack Evands.Pellucid.nuspec -Version $ver1 -OutputDirectory $outPath

    if ([System.IO.File]::Exists("$outPath/Evands.Pellucid.$ver1.nupkg") -eq $false)
    {
        Write-Warning "Unable to create nuget package for Evands.Pellucid"
        $exitCode = 1002
    }
    else
    {
        Write-Host "Creating Nuget Package for Evands.Pellucid.Pro"
        (Get-Content -Path "Evands.Pellucid.Pro.nuspec") -Replace "depVer", $ver1 | Set-Content -Path "Evands.Pellucid.Pro.temp.nuspec"
        nuget pack Evands.Pellucid.Pro.temp.nuspec -Version $ver2 -OutputDirectory $outPath
        if ([System.IO.File]::Exists("$outPath/Evands.Pellucid.Pro.$ver2.nupkg") -eq $false)
        {
            Write-Warning "Unable to create nuget package for Evands.Pellucid.Pro"
            $exitCode = 1003
        }

        if ([System.IO.File]::Exists("$PSScriptRoot/Evands.Pellucid.Pro.temp.nuspec") -eq $true)
        {
            Remove-Item -Path "Evands.Pellucid.Pro.temp.nuspec"
        }
    }

    if ($exitCode -eq 0)
    {
        Write-Host "Creating release archive."
        $libDll = "$PSScriptRoot/../src/Evands.Pellucid/bin/Release/Evands.Pellucid.dll"
        $libXml = "$PSScriptRoot/../src/Evands.Pellucid/bin/Release/Evands.Pellucid.xml"
        $libProDll = "$PSScriptRoot/../src/Evands.Pellucid.Pro/bin/Release/Evands.Pellucid.Pro.dll"
        $libProXml = "$PSScriptRoot/../src/Evands.Pellucid.Pro/bin/Release/Evands.Pellucid.Pro.xml"
        $demo = "$PSScriptRoot/../src/Evands.Pellucid.ProDemo/bin/Release/Evands.Pellucid.ProDemo.cpz"

        $archivePath = "$outPath/Evands.Pellucid-Crestron-v$ver1.zip"

        try {
            Compress-Archive $libDll, $libXml, $libProDll, $libProXml, $demo -DestinationPath $archivePath
            Write-Host "Created release archive."
        }
        catch {
        }

        if ([System.IO.File]::Exists($archivePath) -eq $false)
        {
            Write-Host "Error creating the release archive."
            $exitCode = 1004
        }
    }
}

exit $exitCode