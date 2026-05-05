# Defines the mode the compilation will use.
param(
[string]$Mode
)

if ($Mode -ne "Debug" -and $Mode -ne "Test" -and $Mode -ne "Release")
{
    Write-Host "Invalid Mode Specified. Unable to Build." -ForegroundColor Red
    exit 1001
}

$path = Split-Path $PSScriptRoot -Parent
$slnPath = "$path/src/Evands.Pellucid.slnx"

Write-Host "Building solution in $Mode mode..."
dotnet build $slnPath --configuration $Mode

if ($LASTEXITCODE -ne 0)
{
    Write-Error -Message "Failed to build project." -Category InvalidResult -ErrorId 1000
    exit 1000
}
else {
    Write-Host "Solution Build Successful" -ForegroundColor Green
}