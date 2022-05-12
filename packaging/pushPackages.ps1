param(
    [string]$SearchPath,
    [string]$ApiKey
)

$items = Get-ChildItem $SearchPath | Get-Unique

$feedUrl = 'https://api.nuget.org/v3/index.json'

foreach ($item in $items)
{
    $itemName = $item.FullName
    Write-Host "Publishing package $itemName"
    nuget push $itemName -Source $feedUrl -ApiKey $ApiKey
    Write-Host "Published $itemName"
}

Write-Host "Published packages."