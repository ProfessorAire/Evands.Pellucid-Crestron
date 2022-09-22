# Defines the mode the compilation will use.
param(
[string]$Mode
)

if ($Mode -ne "Debug" -and $Mode -ne "Test" -and $Mode -ne "Release")
{
    Write-Host "Invalid Mode Specified. Unable to Build." -ForegroundColor Red
    exit 1001
}

$slnPath = "$PSScriptRoot/../src/Series4/Evands.Pellucid.sln"
$MsBuild = "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"

& "$MsBuild" -p:Configuration=$Mode -restore $slnPath