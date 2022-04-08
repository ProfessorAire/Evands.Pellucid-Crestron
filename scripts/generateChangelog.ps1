param(
    [Parameter(Mandatory = $false)][switch]$IncludeOtherChanges
)

function Get-Numbers
{
    param(
        [Parameter()][string]$Type,
        [Parameter()][string]$Scope,
        [Parameter()][string]$Message
    )

    $numPattern = "(\({0,1}(?<Num>#\d+)\){0,1})+"
    $TypeMatch = select-string -InputObject $Type -Pattern $numPattern -AllMatches
    $ScopeMatch = select-string -InputObject $Scope -Pattern $numPattern -AllMatches
    $MessageMatch = select-string -InputObject $Message -Pattern $numPattern -AllMatches

    $nums = [System.Collections.Generic.SortedSet[string]]::new()

    if ($TypeMatch.Matches.Count -gt 0)
    {
        foreach($match in $TypeMatch.Matches)
        {
            $nums.Add($match.Groups["Num"].Value) | Out-Null
        }
    }
    
    if ($ScopeMatch.Matches.Count -gt 0)
    {
        foreach($match in $ScopeMatch.Matches)
        {
            $nums.Add($match.Groups["Num"].Value) | Out-Null
        }
    }
    
    if ($MessageMatch.Matches.Count -gt 0)
    {
        foreach($match in $MessageMatch.Matches)
        {
            $nums.Add($match.Groups["Num"].Value) | Out-Null
        }
    }

    $nums
}

function WriteContent
{
    param(
        [Parameter()][string]$Type,
        [Parameter()][string]$Scope,
        [Parameter()][string]$Message
    )

    $nums = Get-Numbers $Type $Scope $Message

    $printNum = ""
    foreach ($num in $nums)
    {
        $Scope = $Scope.Replace("($num)", "").Replace("$num", "").Trim()
        if ($printNum.Length -gt 0)
        {
            $printNum = $printNum + ", "
            $printNum = $printNum + $num
        }
    }

    $Message = $Message.Trim()

    $start = -1
    $idx = 0
    while ($start -eq -1)
    {
        if ([char]::IsDigit($Message[$idx]) -or $Message[$idx].ToString() -eq "(" -or $Message[$idx].ToString() -eq ")" -or $Message[$idx].ToString() -eq "#")
        {
            $idx++
        }
        else {
            $start = $idx            
        }
    }

    if ($start -gt -1)
    {
        $Message = $Message.Substring($start, $Message.Length - $start).Replace(" - ", "").Trim()
    }
    
        $content = ""

    if ($nums.Length -gt 0)
    {
        $content = "$nums - "
    }
    if ($Scope -ne "")
    {
        Write-Host "$Type($Scope): $Message"
        $content += "($Scope`) $Message"
    }
    else
    {
        $content += "$Message"
        Write-Host "${Type}: $Message"
    }

    $content += "`r`n"
    "* $content"
}

$tags = git tag --list 'v*.*.*' -i
$tag = $tags[$tags.Length - 1]
Write-Host "Found the tag '$tag'"

$filter = "$tag..HEAD"

$gititems = (git log $filter --format="%ai`t%H`t%an`t%ae`t%s`t%b%n")

$gather = [System.Collections.ArrayList]@()
$idx = 0;

foreach ($item in $gititems)
{
    if ($item -ne "")
    {
        if ($gather.Count -le $idx)
        {
            $gather.Add($item)
        }
        else {
            $gather[$idx] += "%n%" + $item
        }
    }
    else
    {
        if ($idx -lt $gather.Count)
        {
            $idx++
        }
    }
}

$gitlogs =  $gather | ConvertFrom-Csv -Delimiter "`t" -Header ("Date", "CommitId", "Author", "Email", "Subject", "Body")

Write-Host "Found $count log items."

$fixes = [System.Collections.ArrayList]@()
$feats = [System.Collections.ArrayList]@()
$breaks = [System.Collections.ArrayList]@()
$others = [System.Collections.ArrayList]@()

$pattern1 = "(?m)^(?<Type>[A-Za-z0-9]+) ?(?>\((?<Scope>[^)]*)\))?(?<Break>!)?: (?<Msg>[\s\S\r\n]+)$"
$pattern2 = "^\(?(?<Num>#\d+)\)?[ :]{0,3}(?<Msg>.*)$"

foreach ($item in $gitlogs)
{
    if ($item.Subject -match $pattern1)
    {
        $type = $Matches.Type.ToLower()       
        
        if ($Matches.Break -or $Matches.Msg -contains "BREAKING CHANGE:")
        {
            $nums = Get-Numbers $Matches.Type $Matches.Scope $Matches.Msg
            $value = [PSCustomObject]@{
                Type = $Matches.Type
                Scope = $Matches.Scope
                Msg = $Matches.Msg
                Numbers = $nums
                Body = $item.Body -replace "%n%", "`r`n"
            }

            $count = $breaks.Add($value)
        }
        elseif ($type -eq "fix" -or $type -eq "bug")
        {
            $nums = Get-Numbers $Matches.Type $Matches.Scope $Matches.Msg
            $value = [PSCustomObject]@{
                Type = $Matches.Type
                Scope = $Matches.Scope
                Msg = $Matches.Msg
                Numbers = $nums
                Body = $item.Body -replace "%n%", "`r`n"
            }

            $count = $fixes.Add($value)
        }
        elseif ($type -eq "feat" -or $type -eq "feature")
        {
            $nums = Get-Numbers $Matches.Type $Matches.Scope $Matches.Msg
            $value = [PSCustomObject]@{
                Type = $Matches.Type
                Scope = $Matches.Scope
                Msg = $Matches.Msg
                Numbers = $nums
                Body = $item.Body -replace "%n%", "`r`n"
            }

            $count = $feats.Add($value)
        }
        elseif ($IncludeOtherChanges -eq $true)
        {
            $nums = Get-Numbers $Matches.Type $Matches.Scope $Matches.Msg
            $value = [PSCustomObject]@{
                Type = $Matches.Type
                Scope = $Matches.Scope
                Msg = $Matches.Msg
                Numbers = $nums
                Body = $item.Body -replace "%n%", "`r`n"
            }

            $count = $others.Add($value)
        }
    }
    elseif ($item.Subject -match $pattern2)
    {
        $count = $unknown.Add(($Matches.Num, $Matches.Msg))
    }
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