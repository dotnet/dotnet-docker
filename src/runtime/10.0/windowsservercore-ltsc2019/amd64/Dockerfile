# escape=`

# Installer image
FROM mcr.microsoft.com/windows/servercore:ltsc2019-amd64 AS installer

# Retrieve .NET Runtime
RUN powershell -Command " `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        $dotnetFile = \"dotnet-runtime-10.0.0-preview.6.25326.107-win-x64.zip\"; `
        $dotnetSha512File = \"dotnet-runtime-10.0.0-preview.6.25326.107-win-x64.zip.sha512\"; `
        `
        Invoke-WebRequest -OutFile $dotnetFile https://ci.dot.net/public/Runtime/10.0.0-preview.6.25326.107/dotnet-runtime-10.0.0-preview.6.25326.107-win-x64.zip; `
        Invoke-WebRequest -OutFile $dotnetSha512File https://ci.dot.net/public-checksums/Runtime/10.0.0-preview.6.25326.107/dotnet-runtime-10.0.0-preview.6.25326.107-win-x64.zip.sha512; `
        `
        if ((Get-FileHash $dotnetFile -Algorithm sha512).Hash -ne (Get-Content $dotnetSha512File)) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        mkdir dotnet; `
        tar --gzip --extract --no-same-owner --file $dotnetFile --directory dotnet; `
        Remove-Item -Force $dotnetFile, $dotnetSha512File"


# Runtime image
FROM mcr.microsoft.com/windows/servercore:ltsc2019-amd64

ENV `
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_HTTP_PORTS=8080 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true `
    # .NET Runtime version
    DOTNET_VERSION=10.0.0-preview.6.25326.107

RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"

COPY --from=installer ["/dotnet", "/Program Files/dotnet"]
