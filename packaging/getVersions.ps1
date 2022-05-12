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

function Get-VersionData
{
    param(
        [string]$filePath
    )

    $content = Get-Content $filePath -Raw
    $match = $content -match 'AssemblyVersion\("(?<ver>\d+\.\d+\.\d+\.\d+)"\)'

    if ($match) {
        return $matches["ver"]
    }
    
    Write-Error "Unable to get version data for '$filePath'"
    exit 1001
}

$pellucidPath = "$PSScriptRoot/../src/Evands.Pellucid/Properties/AssemblyInfo.cs"
$pellucidProPath = "$PSScriptRoot/../src/Evands.Pellucid.Pro/Properties/AssemblyInfo.cs"
$pellucidCwsPath = "$PSScriptRoot/../src/Evands.Pellucid.CwsConsole/Properties/AssemblyInfo.cs"

$libVersion = Get-VersionData $pellucidPath
Write-Host "Lib Raw Version: $libVersion"
$proVersion = Get-VersionData $pellucidProPath
Write-Host "Pro Raw Version: $proVersion"
$cwsVersion = Get-VersionData $pellucidCwsPath
Write-Host "Cws Raw Version: $cwsVersion"

if ($libVersion -eq "" -or $proVersion -eq "" -or $cwsVersion -eq "")
{
    Write-Host "Unable to find version information, cannot create packages."
    exit 1001
}
else
{
    $ver1 = Get-LibVersion $libVersion
    $ver2 = Get-LibVersion $proVersion
    $ver3 = Get-LibVersion $cwsVersion

    Write-Host "Lib Version Found: $ver1"
    Write-host "Pro Version Found: $ver2"
    Write-Host "Cws Version Found: $ver3"

    $preRelease = "false"
    $cwsPre = "false"

    if ($ver1.IndexOf("-") -gt -1)
    {
        $preRelease = "true"
    }

    if ($ver3.IndexOf("-") -gt -1)
    {
        $cwsPre = "true"
    }

    return $ver1, $libVersion, $ver2, $proVersion, $preRelease, $ver3, $cwsVersion, $cwsPre
}