# escape=`

# Installer image
FROM mcr.microsoft.com/windows/servercore:ltsc2025-amd64 AS installer

# Retrieve .NET Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        $dotnet_version = '10.0.1'; `
        $dotnet_file = 'dotnet-runtime-' + $dotnet_version + '-win-x64.zip'; `
        $dotnet_sha512_file = $dotnet_file + '.sha512-bare'; `
        `
        Invoke-WebRequest -OutFile $dotnet_file https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/$dotnet_file; `
        Invoke-WebRequest -OutFile $dotnet_sha512_file https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/$dotnet_sha512_file; `
        `
        if ((Get-FileHash $dotnet_file -Algorithm sha512).Hash -ne (Get-Content $dotnet_sha512_file)) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        mkdir dotnet; `
        tar --gzip --extract --no-same-owner --file $dotnet_file --directory dotnet; `
        Remove-Item -Force `
            $dotnet_file, `
            $dotnet_sha512_file


# Runtime image
FROM mcr.microsoft.com/windows/nanoserver:ltsc2025-amd64

ENV `
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_HTTP_PORTS=8080 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true `
    # .NET Runtime version
    DOTNET_VERSION=10.0.1

# In order to set system PATH, ContainerAdministrator must be used
USER ContainerAdministrator
RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"
USER ContainerUser

COPY --from=installer ["/dotnet", "/Program Files/dotnet"]
