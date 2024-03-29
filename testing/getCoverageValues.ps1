[String]$html = Get-Content -Path $PSScriptRoot/coverage/index.html

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