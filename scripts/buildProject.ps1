# Defines the mode the compilation will use.
param(
[string]$Mode
)

if ($Mode -ne "Debug" -and $Mode -ne "Test" -and $Mode -ne "Release")
{
    Write-Host "Invalid Mode Specified. Unable to Build." -ForegroundColor Red
    exit 1001
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

$dte.Solution.SolutionBuild.SolutionConfigurations[$Mode].Activate()
Write-Host "Active Configuration is " $dte.Solution.SolutionBuild.ActiveConfiguration.Name

if ($dte.Solution.SolutionBuild.ActiveConfiguration.Name -eq $Mode)
{
    Write-Host "Cleaning the solution."
    $dte.Solution.SolutionBuild.Clean($true)
    
    Write-Host "Building Project"
    $dte.Solution.SolutionBuild.Build($true)
    $failed = $dte.Solution.SolutionBuild.LastBuildInfo

    "$failed projects failed to build."

    if ($failed -gt 0)
    {
        Write-Error -Message "Failed to build project." -Category InvalidResult -ErrorId 1000
        $exitCode = 1000
        exit $exitCode
    }
}

$dte.Solution.Close($false)
$dte.Quit()

Set-Content -Path "$PSScriptRoot/../src/Evands.Pellucid/Evands.Pellucid.csproj" -Value $libContents
Set-Content -Path "$PSScriptRoot/../src/Evands.Pellucid.Pro/Evands.Pellucid.Pro.csproj" -Value $libProContents
Set-Content -Path "$PSScriptRoot/../src/Evands.Pellucid.ProDemo/Evands.Pellucid.ProDemo.csproj" -Value $libProDemoContents