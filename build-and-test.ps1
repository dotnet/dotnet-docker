[cmdletbinding()]
param(
    [switch]$UseImageCache,
    [string]$Filter,
    [string]$Architecture
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$(docker version) | % { Write-Host "$_" }

if ($UseImageCache) {
    $optionalDockerBuildArgs = ""
}
else {
    $optionalDockerBuildArgs = "--no-cache"
}

$manifest = Get-Content "manifest.json" | ConvertFrom-Json
$manifestRepo = $manifest.Repos[0]
$activeOS = docker version -f "{{ .Server.Os }}"
$builtTags = @()

$manifestRepo.Images |
    ForEach-Object {
        $images = $_
        $_.Platforms |
            Where-Object { $_.os -eq "$activeOS" } |
            Where-Object { [string]::IsNullOrEmpty($Filter) -or $_.dockerfile -like "$Filter" } |
            Where-Object { ( [string]::IsNullOrEmpty($Architecture) -and -not [bool]($_.PSobject.Properties.name -match "architecture"))`
                -or ( [bool]($_.PSobject.Properties.name -match "architecture") -and $_.architecture -eq "$Architecture" ) } |
            ForEach-Object {
                $dockerfilePath = $_.dockerfile
                $tags = [array]($_.Tags | ForEach-Object { $_.PSobject.Properties })
                if ([bool]($images.PSobject.Properties.name -match "sharedtags")) {
                    $tags += [array]($images.sharedtags | ForEach-Object { $_.PSobject.Properties })
                }

                $qualifiedTags = $tags | ForEach-Object {
                    $manifestRepo.Name + ':' + $_.name.Replace('$(nanoServerVersion)', $manifest.TagVariables.NanoServerVersion)
                }
                $formattedTags = $qualifiedTags -join ', '
                Write-Host "--- Building $formattedTags from $dockerfilePath ---"
                Invoke-Expression "docker build $optionalDockerBuildArgs -t $($qualifiedTags -join ' -t ') $dockerfilePath"
                if ($LastExitCode -ne 0) {
                    throw "Failed building $formattedTags"
                }

                $builtTags += $formattedTags
            }
    }

./test/run-test.ps1 -Filter $Filter -Architecture $Architecture

Write-Host "Tags built and tested:`n$($builtTags | Out-String)"
