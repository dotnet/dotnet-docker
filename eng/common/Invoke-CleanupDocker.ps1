Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

docker ps -a -q | ForEach-Object { docker rm -f $_ }

docker volume prune -f

# Windows base images are large, preserve them to avoid the overhead of pulling each time.
docker images |
    Where-Object {
        -Not ($_.StartsWith("mcr.microsoft.com/windows")`
        -Or $_.StartsWith("REPOSITORY ")) } |
    ForEach-Object { $_.Split(' ', [System.StringSplitOptions]::RemoveEmptyEntries)[2] } |
    Select-Object -Unique |
    ForEach-Object { docker rmi -f $_ }
