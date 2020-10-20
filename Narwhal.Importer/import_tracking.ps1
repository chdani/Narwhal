## Get the storage directory from the environment
$storageDirectory = $Env:STORAGE_DIRECTORY
if ([string]::IsNullOrWhiteSpace($storageDirectory)) {
    $storageDirectory = "$PSScriptRoot\..\Playground\Storage"
}

## We're looking at the tracks of a couple of vessels in 2018
$vessels = 840602, 3476536, 480772, 1857943, 6352509

## Download tracking positions
Foreach ($vessel in $vessels)
{
    $url = "https://gateway.api.globalfishingwatch.org/v2/tilesets/gfw-tasks-1095-vessel-id-logistic-v1/sub/seriesgroup=" + $vessel + "/2018-01-01T00:00:00.000Z,2019-01-01T00:00:00.000Z;0,0,0"
    $fileName = "tracking_" + $vessel + "_2018.vectortile"
    $filePath = Join-Path $storageDirectory $fileName

    Write-Host "Downloading data file from", $url, "to", $filePath

    if (Test-Path -LiteralPath $filePath -PathType Leaf) {
        Remove-Item -LiteralPath $filePath
    }

    (New-Object System.Net.WebClient).DownloadFile($url, $filePath)
}

Write-Host "All good :)"