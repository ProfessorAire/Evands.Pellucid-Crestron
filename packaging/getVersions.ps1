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

$libVersion = ""
$proVersion = ""

Write-Host "Looking for version information"
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
                                    Write-Host "Evands.Pellucid version found: $ver"
                                    $libVersion = $ver
                                }
                                elseif ($proj.Name -eq "Evands.Pellucid.Pro")
                                {
                                    Write-Host "Evands.Pellucid.Pro version found: $ver"
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

if ($libVersion -eq "" -or $proVersion -eq "")
{
    Write-Host "Unable to find version information, cannot create packages."
    exit 1001
}
else
{
    $ver1 = Get-LibVersion $libVersion
    $ver2 = Get-LibVersion $proVersion

    $preRelease = "false"

    if ($ver1 -contains "-")
    {
        $preRelease = "true"
    }

    return $ver1, $libVersion, $ver2, $proVersion, $preRelease
}