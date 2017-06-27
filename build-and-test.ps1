[cmdletbinding()]
param(
    [switch]$UseImageCache
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

if ($UseImageCache) {
    $optionalDockerBuildArgs = ""
}
else {
    $optionalDockerBuildArgs = "--no-cache"
}

$manifest = Get-Content "manifest.json" | ConvertFrom-Json
$manifestRepo = $manifest.Repos[0]
$platform = docker version -f "{{ .Server.Os }}"
$builtTags = @()

$manifestRepo.Images |
    ForEach-Object {
        $images = $_
        $_.Platforms |
            Where-Object {[bool]($_.PSobject.Properties.name -match $platform)} |
            ForEach-Object {
                $dockerfilePath = $_.$platform.dockerfile
                $tags = $_.$platform.Tags
                if ([bool]($images.PSobject.Properties.name -match "sharedTags")) {
                    $tags += $images.sharedTags
                }

                $qualifiedTags = $tags | ForEach-Object {
                    $manifestRepo.Name + ':' + $_.Replace('$(nanoServerVersion)', $manifest.TagVariables.NanoServerVersion)
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

./test/run-test.ps1 -UseImageCache:$UseImageCache

Write-Host "Tags built and tested:`n$($builtTags | Out-String)"
