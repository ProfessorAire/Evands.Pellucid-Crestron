param(
[string]$Series
)

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

if ($Series -ne "3" -And $Series -ne "4")
{
    Write-Warning "You must provide an appropriate series in the mode of '-Series 3' or '-Series 4'. Unable to continue."
    exit 1004
}

$Series = "Series$Series"
$path = Split-Path $PSScriptRoot -Parent
$sourcePath = "$path\src\$Series\Evands.Pellucid.Tests\bin\Test\Evands.Pellucid.Tests.dll"
$resultPath = "$PSScriptRoot\$Series\testResults.trx"
$coveragePath = "$PSScriptRoot\$Series\coverageResults.xml"
$coverageHtmlPath = "$PSScriptRoot\$Series\coverage\"

if ($Series -eq "Series3")
{
    $testExe = "C:\Program Files (x86)\Microsoft Visual Studio 9.0\Common7\IDE\MSTest.exe"
}
else
{
    $testExe = "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"
}

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
    Write-Warning "Unable to locate the $testExe application. Unable to continue."
    exit 1003
}

if ($null -ne (Get-Item $resultPath -ErrorAction SilentlyContinue)) {
    Remove-Item $resultPath    
}

if ($null -ne (Get-Item "$PSScriptRoot\$Series" -ErrorAction SilentlyContinue)) {
    Remove-Item "$PSScriptRoot\$Series" -Recurse -Force
}

New-Item "$PSScriptRoot\$Series" -ItemType "Directory" -Force

if ($Series -eq "Series3")
{
    & $openCover "-target:$testExe" "-targetargs:/testContainer:$sourcePath /resultsfile:$resultPath" -log:all -output:$coveragePath -filter:"+[Evands*]* -[Evands.Pellucid.Tests*]* -[Evands.Pellucid.Fakes*]*"
}
else
{
    & $openCover "-target:$testExe" "-targetargs:$sourcePath --Logger:""trx;LogFileName=$resultPath""" -log:all -output:$coveragePath -filter:"+[Evands*]* -[Evands.Pellucid.Tests*]* -[Evands.Pellucid.Fakes*]*"
}

& $reportGenerator -reports:$coveragePath -targetdir:$coverageHtmlPath -reporttypes:Html

$dir = Get-ChildItem "$PSScriptRoot/$Series" -Directory

foreach($item in $dir)
{
    if ($item.Name -ne 'coverage')
    {
        DeleteAll $item.FullName
        $item.Delete($true)
    }
}