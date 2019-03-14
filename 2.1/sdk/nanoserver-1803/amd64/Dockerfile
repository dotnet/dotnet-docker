# escape=`

# Installer image
FROM mcr.microsoft.com/windows/servercore:1803 AS installer

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Retrieve .NET Core SDK
ENV DOTNET_SDK_VERSION 2.1.505

RUN Invoke-WebRequest -OutFile dotnet.zip https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$Env:DOTNET_SDK_VERSION/dotnet-sdk-$Env:DOTNET_SDK_VERSION-win-x64.zip; `
    $dotnet_sha512 = 'aaa2b691f3ac7bdc1ba7edcfabb4c51b53fe1760db529f3ab6e77f0436627b67940f10014d10f3f76b0c77a5f4289b15dc1e525ced1e0c4f0406fd52d9ddb682'; `
    if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    Expand-Archive dotnet.zip -DestinationPath dotnet; `
    Remove-Item -Force dotnet.zip

# Install PowerShell global tool
ENV POWERSHELL_VERSION 6.2.0-rc.1

# The global tool package PowerShell.Windows only supports win10-x64, win-arm, win-x64 runtimes.
RUN Invoke-WebRequest -OutFile "PowerShell.Windows.$ENV:POWERSHELL_VERSION.nupkg" "https://pwshtool.blob.core.windows.net/v6-2-0-rc-1/PowerShell.Windows.$ENV:POWERSHELL_VERSION.nupkg"; `
    $powershell_sha512 = '404b81c7d1cdd79556db560eaf1ab46c80d7d0d006e91a1fa3eccd00032a0415b63c6c3c2a6f386881b9de7b62ca4dc46f11ab3d941690564578961b947716ce'; `
    if ((Get-FileHash "PowerShell.Windows.$ENV:POWERSHELL_VERSION.nupkg" -Algorithm sha512).Hash -ne $powershell_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    \dotnet\dotnet tool install --add-source . --tool-path '\powershell' --version "$ENV:POWERSHELL_VERSION" PowerShell.Windows; `
    Remove-Item -Force "PowerShell.Windows.$ENV:POWERSHELL_VERSION.nupkg"

# SDK image
FROM mcr.microsoft.com/windows/nanoserver:1803

COPY --from=installer ["/dotnet", "/Program Files/dotnet"]

COPY --from=installer ["/powershell", "/Program Files/powershell"]

# In order to set system PATH, ContainerAdministrator must be used
USER ContainerAdministrator
RUN setx /M PATH "%PATH%;C:\Program Files\dotnet;C:\Program Files\powershell"
USER ContainerUser

# Configure web servers to bind to port 80 when present
ENV ASPNETCORE_URLS=http://+:80 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true `
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true `
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip

# Trigger first run experience by running arbitrary cmd to populate local package cache
RUN dotnet help
