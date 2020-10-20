## Get the storage directory from the environment
$storageDirectory = $Env:STORAGE_DIRECTORY
if ([string]::IsNullOrWhiteSpace($storageDirectory)) {
    $storageDirectory = "$PSScriptRoot\..\Playground\Storage"
}

## Create the file name
$now = Get-Date
$fileName = $now.ToString("yyyy-MM-dd") + ".txt"
$filePath = Join-Path $storageDirectory $fileName

## Download the data from the online source
$url = "https://gan.shom.fr/navarea/NavareaIIenVigueur.txt"
Write-Host "Downloading data file from", $url, "to", $filePath

if (Test-Path -LiteralPath $filePath -PathType Leaf) {
    Remove-Item -LiteralPath $filePath
}
(New-Object System.Net.WebClient).DownloadFile($url, $filePath)

Write-Host "All good :)"