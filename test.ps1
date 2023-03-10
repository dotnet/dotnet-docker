param(
    [switch]$noCache = $false,
    [switch]$buildSample = $false,
    [switch]$runSample = $false
)

set-psdebug -trace 1

$extraArgs = @()
if ($noCache) {
    $extraArgs += "--no-cache"
}

push-location $PSScriptRoot\src\runtime-deps\8.0\bookworm-slim\amd64
docker build -t runtime-deps `
    -f .\Dockerfile `
    @extraArgs .
pop-location

push-location $PSScriptRoot\src\runtime\8.0\bookworm-slim\amd64
docker build -t runtime `
    -f .\Dockerfile `
    --build-arg BASE_IMAGE=runtime-deps `
    @extraArgs .
pop-location

push-location $PSScriptRoot\src\aspnet\8.0\bookworm-slim\amd64
docker build -t aspnet-composite `
    -f .\Dockerfile `
    --build-arg BASE_IMAGE=runtime-deps `
    @extraArgs .
pop-location

push-location $PSScriptRoot\src\sdk\8.0\bookworm-slim\amd64
docker build -t sdk-composite `
    -f .\Dockerfile `
    --build-arg BASE_IMAGE=aspnet-composite `
    @extraArgs .
pop-location

push-location $PSScriptRoot\src\sdk\8.0\bookworm-slim\amd64
docker build -t sdk-runtime `
    -f .\Dockerfile `
    --build-arg BASE_IMAGE=runtime `
    @extraArgs .
pop-location

if ($buildSample) {
    push-location $PSScriptRoot\samples\aspnetapp
    docker build -t sample `
        -f .\Dockerfile `
        --build-arg SDK_IMAGE=sdk-runtime `
        --build-arg RUNTIME_IMAGE=aspnet-composite `
        @extraArgs .
    pop-location
}

if ($runSample) {
    docker run --rm -p 8080:8080 sample
}
