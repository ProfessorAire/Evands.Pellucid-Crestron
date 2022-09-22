param(
[string]$Series
)

if ($Series -ne "3" -And $Series -ne "4")
{
    Write-Warning "You must provide an appropriate series in the mode of '-Series 3' or '-Series 4'. Unable to continue."
    exit 1004
}

if ($Series -eq "3")
{
    [String]$html = Get-Content -Path $PSScriptRoot/Series3/coverage/index.html
}
else
{
    [String]$html = Get-Content -Path $PSScriptRoot/Series4/coverage/index.html
}

$result = $html -match 'Line coverage<\/div>\n? ?<div.*?>\n? ?<div.*?>(\d{1,3}\.?\d{0,2})%'

if ($result -eq $true)
{
    $linePercent = $Matches[1]
    $color = "red"

    if ($linePercent -gt 40 -and $linePercent -lt 70)
    {
        $color = "yellow"
    }
    elseif ($linePercent -ge 70 -and $linePercent -lt 85)
    {
        $color = "yellowgreen"
    }
    elseif ($linePercent -ge 85 -and $linePercent -lt 95)
    {
        $color = "green"
    }
    elseif ($linePercent -ge 95)
    {
        $color = "brightgreen"
    }

    return "$linePercent%", $color
}
else
{
    Write-Error "Unable to determine the code coverage from the HTML provided."
    exit 1000
}