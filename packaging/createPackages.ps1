function Get-LibVersion {
    param(
        [string]$version
    )

    $parts = $version.Split(".")
    $part0 = $parts[0]
    $part1 = $parts[1]
    $part2 = $parts[2]
    $part3 = [int]$parts[3].Trim("`"")

    $lastPart = ""

    if ($part3 -ge 100 -and $part3 -lt 200) {
        $lastPart = "Alpha." + ($part3 - 99).ToString()
    }
    elseif ($part3 -ge 200 -and $part3 -lt 300) {
        $lastPart = "Beta." + ($part3 - 199).ToString()
    }
    elseif ($part3 -ge 300 -and $part3 -lt 1000) {
        $lastPart = "RC." + ($part3 - 299).ToString()
    }

    [string]$ver = "$part0.$part1.$part2"

    if ($lastPart -ne "") {
        $ver = "$ver-$lastPart"
    }

    return [string]$ver.Replace("`"", "")
}

"Creating new Visual Studio DTE Object"
$dte = New-Object -ComObject VisualStudio.DTE.9.0

$path = Split-Path $PSScriptRoot -Parent
$path = "$path\src\EVS.Pellucid.sln"

"Attempting to open $path"
$dte.Solution.Open($path)

"Cleaning the solution."
$dte.Solution.SolutionBuild.Clean($true)

foreach ($item in $dte.Solution.SolutionBuild.SolutionConfigurations) {
    if ($item.Name -eq "Release") {
        $item.Activate()
        break
    }
}

"Active Configuration is " + $dte.Solution.SolutionBuild.ActiveConfiguration.Name
if ($dte.Solution.SolutionBuild.ActiveConfiguration.Name -eq "Release") {
    $dte.Solution.SolutionBuild.Build($true)
}

$libVersion = ""
$proVersion = ""

foreach ($proj in $dte.Solution.Projects) {
    foreach ($rootItem in $proj.ProjectItems) {
        if ($rootItem.Name -eq "Properties") {
            foreach ($subItem in $rootItem.ProjectItems) {
                if ($subItem.Name -eq "AssemblyInfo.cs") {
                    foreach ($element in $subItem.FileCodeModel.CodeElements) {
                        if ($element.Kind -eq 7) {
                            $name = $element.Name
                            $ver = $element.Value

                            if ($name -eq "AssemblyVersion") {
                                if ($proj.Name -eq "EVS.Pellucid") {
                                    $libVersion = $ver
                                }
                                elseif ($proj.Name -eq "EVS.Pellucid.Pro") {
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

$exitCode = 0

if ($libVersion -eq "" -or $proVersion -eq "") {
    "Unable to find version information, cannot create packages."
    $exitCode = 1001
}
else {
    $ver1 = Get-LibVersion $libVersion
    $ver2 = Get-LibVersion $proVersion
    
    "Final EVS.Pellucid Version: $ver1"
    "Final EVS.Pellucid.Pro Version: $ver2"

    "Creating Nuget Package for EVS.Pellucid"
    nuget pack EVS.Pellucid.nuspec -Version $ver1 -OutputDirectory ./releases

    if ([System.IO.File]::Exists("$PSScriptRoot/releases/EVS.Pellucid.$ver1.nupkg") -eq $false)
    {
        Write-Warning "Unable to create nuget package for EVS.Pellucid"
        $exitCode = 1002
    }
    else {
        "Creating Nuget Package for EVS.Pellucid.Pro"
        nuget pack EVS.Pellucid.Pro.nuspec -Version $ver2 -OutputDirectory ./releases
        if ([System.IO.File]::Exists("$PSScriptRoot/releases/EVS.Pellucid.Pro.$ver2.nupkg") -eq $false)
        {
            Write-Warning "Unable to create nuget package for EVS.Pellucid.Pro"
            $exitCode = 1003
        }
    }
}

$dte.Solution.Close($false)
$dte.Quit()
exit $exitCode