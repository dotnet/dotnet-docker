# escape=`

# Installer image
FROM mcr.microsoft.com/windows/servercore:ltsc2019-amd64 AS installer

# Retrieve .NET Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        $dotnet_version = '9.0.6'; `
        Invoke-WebRequest -OutFile dotnet.zip https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-win-x64.zip; `
        $dotnet_sha512 = 'dbc510d2391b34bcad62a622b65ff32c1ea3a210a4d80e084e93cf0c47bbcfa53a5aedbfcfeb9641f23a90abc63eb31da2107711ff5bde0d48d51645eb12bece'; `
        if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        mkdir dotnet; `
        tar --gzip --extract --no-same-owner --file dotnet.zip --directory dotnet; `
        Remove-Item -Force dotnet.zip


# Runtime image
FROM mcr.microsoft.com/windows/servercore:ltsc2019-amd64

ENV `
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_HTTP_PORTS=8080 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true `
    # .NET Runtime version
    DOTNET_VERSION=9.0.6

RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"

COPY --from=installer ["/dotnet", "/Program Files/dotnet"]
