[String]$html = Get-Content -Path coverage/index.html

$result = $html -match '^.*Line coverage:</th><td>(\d{1,3}\.{0,1}\d{0,2})%.*$'

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

    return $linePercent, $color
}
else
{
    return $invalid, $red
}
