# Defines the mode the compilation will use.
param(
[string]$Mode,
[string]$Series
)

if ($Mode -ne "Debug" -and $Mode -ne "Test" -and $Mode -ne "Release")
{
    Write-Host "Invalid Mode Specified. Unable to Build." -ForegroundColor Red
    exit 1001
}

if ($Series -ne "3" -And $Series -ne "4")
{
    Write-Warning "You must provide an appropriate series in the mode of '-Series 3' or '-Series 4'. Unable to continue."
    exit 1004
}

if ($Series -eq "3")
{
    if ($Mode -ne "Test")
    {
    $exitCode = 0

    $libContents = Get-Content -Path "$PSScriptRoot/../src/Series3/Evands.Pellucid/Evands.Pellucid.csproj"
    $libProContents = Get-Content -Path "$PSScriptRoot/../src/Series3/Evands.Pellucid.Pro/Evands.Pellucid.Pro.csproj"
    $libProDemoContents = Get-Content -Path "$PSScriptRoot/../src/Series3/Evands.Pellucid.ProDemo/Evands.Pellucid.ProDemo.csproj"

    Write-Host "Creating new Visual Studio DTE Object"
    $dte = New-Object -ComObject VisualStudio.DTE.9.0

    $dte.SuppressUI = $true

    $path = Split-Path $PSScriptRoot -Parent
    $path = "$path\src\Evands.Pellucid.sln"

    Write-Host "Attempting to open $path"
    $dte.Solution.Open($path)

    while ($dte.Solution.IsOpen -eq $false)
    {
        Write-Host "Waiting for Solution to Open"
        Start-Sleep 3
    }

    $dte.Solution.SolutionBuild.SolutionConfigurations[$Mode].Activate()
    Write-Host "Active Configuration is" $dte.Solution.SolutionBuild.ActiveConfiguration.Name

    if ($dte.Solution.SolutionBuild.ActiveConfiguration.Name -eq $Mode)
    {
        Write-Host "Cleaning the solution..."
        $dte.Solution.SolutionBuild.Clean($true)
        
        Write-Host "Building the Solution..."
        $dte.Solution.SolutionBuild.Build($true)
        $failed = $dte.Solution.SolutionBuild.LastBuildInfo
    }
    else {
        $failed = "All"
    }

    $dte.Solution.Close($false)
    $dte.Quit()

    if ($failed -ne 0)
    {
        Write-Error -Message "Failed to build project. $failed projects failed to build." -Category InvalidResult -ErrorId 1000
        $exitCode = 1000
    }
    else {
        Write-Host "Solution Build Successful" -ForegroundColor Green
    }

    Write-Host "Restoring previous project contents."

    Set-Content -Path "$PSScriptRoot/../src/Series3/Evands.Pellucid/Evands.Pellucid.csproj" -Value $libContents
    Set-Content -Path "$PSScriptRoot/../src/Series3/Evands.Pellucid.Pro/Evands.Pellucid.Pro.csproj" -Value $libProContents
    Set-Content -Path "$PSScriptRoot/../src/Series3/Evands.Pellucid.ProDemo/Evands.Pellucid.ProDemo.csproj" -Value $libProDemoContents

    exit $exitCode
    }
    else {
        $path = Split-Path $PSScriptRoot -Parent
        $path = "$path\src\Series3\Evands.Pellucid.sln"
        & C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe $path -property:Configuration=Test
    }
}
else {    
    $slnPath = "$PSScriptRoot/../src/Series4/Evands.Pellucid.sln"
    $MsBuild = "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"

    & "$MsBuild" -p:Configuration=$Mode -restore $slnPath
}