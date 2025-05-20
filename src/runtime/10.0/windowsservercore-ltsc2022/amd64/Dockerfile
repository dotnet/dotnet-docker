# escape=`

# Installer image
FROM mcr.microsoft.com/windows/servercore:ltsc2022-amd64 AS installer

# Retrieve .NET Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        $dotnet_version = '10.0.0-preview.5.25269.109'; `
        Invoke-WebRequest -OutFile dotnet.zip https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-win-x64.zip; `
        $dotnet_sha512 = '857629e07e13239849768ce7fd0d544c455a8ed4c210b6fdf5aac221104328fce63bcbf374380f9d1d847316f08545d5e09f523f094975bf2b57cb6b1ee47003'; `
        if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        mkdir dotnet; `
        tar -oxzf dotnet.zip -C dotnet; `
        Remove-Item -Force dotnet.zip


# Runtime image
FROM mcr.microsoft.com/windows/servercore:ltsc2022-amd64

ENV `
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_HTTP_PORTS=8080 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true `
    # .NET Runtime version
    DOTNET_VERSION=10.0.0-preview.5.25269.109

RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"

COPY --from=installer ["/dotnet", "/Program Files/dotnet"]
