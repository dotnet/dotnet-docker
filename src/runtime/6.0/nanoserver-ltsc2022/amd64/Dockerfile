# escape=`

# Installer image
FROM mcr.microsoft.com/windows/servercore:ltsc2022-amd64 AS installer

# Retrieve .NET Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        $dotnet_version = '6.0.23'; `
        Invoke-WebRequest -OutFile dotnet.zip https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-win-x64.zip; `
        $dotnet_sha512 = '051794f415fef0356889068cfab9bbb1b2e8cc5c074a5044dfe6a5359e8798b79194c3d961594ada78cc18fade20d30bcb1ce8cf88bddd60494ed43ee7e53a1d'; `
        if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        mkdir dotnet; `
        tar -oxzf dotnet.zip -C dotnet; `
        Remove-Item -Force dotnet.zip


# Runtime image
FROM mcr.microsoft.com/windows/nanoserver:ltsc2022-amd64

ENV `
    # Configure web servers to bind to port 80 when present
    ASPNETCORE_URLS=http://+:80 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true `
    # .NET Runtime version
    DOTNET_VERSION=6.0.23

# In order to set system PATH, ContainerAdministrator must be used
USER ContainerAdministrator
RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"
USER ContainerUser

COPY --from=installer ["/dotnet", "/Program Files/dotnet"]
