# escape=`

# Installer image
FROM mcr.microsoft.com/windows/servercore:ltsc2025-amd64 AS installer

# Retrieve .NET Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        $dotnet_version = '10.0.0-preview.2.25163.2'; `
        Invoke-WebRequest -OutFile dotnet.zip https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-win-x64.zip; `
        $dotnet_sha512 = 'db4f2e6cbf3729461a1232f4f9ffe625c177691082fc06707d12da1fd72e5b1bbf4516bd31fd675ee4eeaeac01bce42030ecc97edf12e36ad721c60ec31f06ad'; `
        if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        mkdir dotnet; `
        tar -oxzf dotnet.zip -C dotnet; `
        Remove-Item -Force dotnet.zip


# Runtime image
FROM mcr.microsoft.com/windows/servercore:ltsc2025-amd64

ENV `
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_HTTP_PORTS=8080 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true `
    # .NET Runtime version
    DOTNET_VERSION=10.0.0-preview.2.25163.2

RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"

COPY --from=installer ["/dotnet", "/Program Files/dotnet"]
