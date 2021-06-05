function DeleteAll
{
    param(
        $Path
    )

    $files = Get-ChildItem $Path -File
    foreach($file in $files)
    {
        Remove-Item $file.FullName -Force
    }

    $dirs = Get-ChildItem $Path -Directory
    foreach($dir in $dirs)
    {
        DeleteAll $dir.FullName
        $dir.Delete($true)
    }
}

$path = Split-Path $PSScriptRoot -Parent
$sourcePath = "$path\src\Evands.Pellucid.Tests\bin\Test\Evands.Pellucid.Tests.dll"
$resultPath = "$PSScriptRoot\testResults.trx"
$coveragePath = "$PSScriptRoot\coverageResults.xml"
$coverageHtmlPath = "$PSScriptRoot\coverage\"

$testExe = "C:\Program Files (x86)\Microsoft Visual Studio 9.0\Common7\IDE\MSTest.exe"
$openCover = "OpenCover.Console.exe"
$reportGenerator = "ReportGenerator.exe"

if ($null -eq (Get-Command $openCover -ErrorAction SilentlyContinue))
{
    Write-Warning "Unable to locate the 'OpenCover.Console.exe' application. Unable to continue."
    exit 1001
}

if ($null -eq (Get-Command $reportGenerator -ErrorAction SilentlyContinue))
{
    Write-Warning "Unable to locate the 'ReportGenerator.exe' application. Unable to continue."
    exit 1002
}

if ($null -eq (Get-Item $testExe -ErrorAction SilentlyContinue))
{
    Write-Warning "Unable to locate the 'MSTest.exe' application. Unable to continue."
    exit 1003
}

if ($null -ne (Get-Item $resultPath -ErrorAction SilentlyContinue)) {
    Remove-Item $resultPath    
}

& $openCover "-target:$testExe" "-targetargs:/testContainer:$sourcePath /resultsfile:$resultPath" -log:all -output:$coveragePath -filter:"+[Evands*]* -[Evands.Pellucid.Tests*]* -[Evands.Pellucid.Fakes*]*"
& $reportGenerator -reports:$coveragePath -targetdir:$coverageHtmlPath -reporttypes:Html

$dir = Get-ChildItem $PSScriptRoot -Directory

foreach($item in $dir)
{
    if ($item.Name -ne 'coverage')
    {
        DeleteAll $item.FullName
        $item.Delete($true)
    }
}