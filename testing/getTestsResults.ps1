[String]$tests = Get-Content -Path $PSScriptRoot/testResults.trx

$result = $tests -match '^.*Counters.*?passed="(\d*)" error="(\d*)" failed="(\d*)"'

if ($result -eq $true)
{
    $passed = $Matches[1]
    $errors = $Matches[2]
    $failed = $Matches[3]

    $color = "brightgreen"

    if ($failed -gt 0 -or $errors -gt 0)
    {
        $color = "red"
    }

    return $passed, $errors, $failed, $color
}
else
{
    Write-Error "Unable to determine the unit tests quantity from the test results provided."
    exit 1000
}
