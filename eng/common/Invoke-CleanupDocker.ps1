Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

docker ps -a -q | ForEach-Object { docker rm -f $_ }

docker volume prune -f

# Preserve the tagged Windows base images and the common eng infra images (e.g. ImageBuilder)
# to avoid the expense of having to repull continuously.
$engInfraImages = Get-Content $PSScriptRoot/templates/variables/docker-images.yml |
    Where-Object { $_.Trim() -notlike 'variables:' } |
    ForEach-Object { $_.Split(' ', [System.StringSplitOptions]::RemoveEmptyEntries)[1] }

docker images --format "{{.Repository}}:{{.Tag}} {{.ID}}"|
    Where-Object {
        $localImage = $_
        $localImage.Contains(":<none> ")`
        -Or -Not ($localImage.StartsWith("mcr.microsoft.com/windows")`
            -Or ($engInfraImages.Where({ $localImage.StartsWith($_) }, 'First').Count -gt 0)) } |
    ForEach-Object { $_.Split(' ', [System.StringSplitOptions]::RemoveEmptyEntries)[1] } |
    Select-Object -Unique |
    ForEach-Object { docker rmi -f $_ }
