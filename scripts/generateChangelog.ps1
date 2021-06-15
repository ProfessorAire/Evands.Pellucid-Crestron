param(
    [Parameter(Mandatory = $false)][switch]$IncludeOtherChanges
)

$tags = git tag --list 'v*.*.*' -i
$tag = $tags[$tags.Length - 1]
Write-Host "Found the tag '$tag'"

$filter = "$tag..HEAD"

$gitlogs = (git log $filter --format="%ai`t%H`t%an`t%ae`t%s`t%s") | ConvertFrom-Csv -Delimiter "`t" -Header ("Date", "CommitId", "Author", "Email", "Subject")

$count = $gitlogs.Length;

Write-Host "Found $count log items."

$fixes = [System.Collections.ArrayList]@()
$feats = [System.Collections.ArrayList]@()
$breaks = [System.Collections.ArrayList]@()
$others = [System.Collections.ArrayList]@()

$pattern = "^(?<Type>[A-Za-z0-9]+) {0,1}\({0,1}(?<Scope>[^)]*)\){0,1}(?<Break>!{0,1}): (?<Msg>[\s\S]+)$"

foreach ($item in $gitlogs)
{
    if ($item.Subject -match $pattern)
    {
        $type = $Matches.Type.ToLower()
        if ($type -eq "fix" -or $type -eq "bug")
        {
            $count = $fixes.Add($Matches)
        }
        elseif ($type -eq "feat" -or $type -eq "feature")
        {
            $count = $feats.Add($Matches)
        }
        elseif ($Matches.Break -eq "!" -or $Matches.Msg -contains "BREAKING CHANGE:")
        {
            $count = $breaks.Add($Matches)
        }
        elseif ($IncludeOtherChanges -eq $true)
        {
            $count = $others.Add($Matches)
        }
    }
}

function WriteContent
{
    param(
        [Parameter()][string]$Type,
        [Parameter()][string]$Scope,
        [Parameter()][string]$Message
    )

    $content = ""
    Write-Host "$Type($Scope): $Message"
    if ($Scope -ne "")
    {
        $content += "($Scope`): "
    }

    $content += "$Message`r`n"
    "* $content"
}

$document = "# Changelog`r`n`r`n"

if ($breaks.Length -gt 0)
{
    Write-Host "Writing Breaking Changes"

    $document += "## Breaking Changes`r`n`r`n"

    foreach ($break in $breaks)
    {
        $document += WriteContent $break.Type $break.Scope $break.Msg
    }

    $document += "`r`n"
}

if ($feats.Length -gt 0)
{
    Write-Host "Writing New Features"

    $document += "## New Features`r`n`r`n"

    foreach ($feat in $feats)
    {
        $document += WriteContent $feat.Type $feat.Scope $feat.Msg
    }

    $document += "`r`n"
}

if ($fixes.Length -gt 0)
{
    Write-Host "Writing Bug Fixes"

    $document += "## Bug Fixes`r`n`r`n"
    
    foreach ($fix in $fixes)
    {
        $document += WriteContent $fix.Type $fix.Scope $fix.Msg
    }

    $document += "`r`n"
}

if ($others.Length -gt 0)
{
    Write-Host "Writing Other Changes"

    $document += "## Other Changes`r`n`r`n"

    foreach ($other in $others)
    {
        $document += WriteContent $other.Type $other.Scope $other.Msg
    }

    $document += "`r`n"
}

if ($document -ne "# Changelog")
{
    Write-Host "Generated Changelog."
    $document.TrimEnd() | Out-File changelog.md
}
else
{
    Write-Error "Unable to generate a changelog."
    exit 1
}