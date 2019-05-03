# escape=`

# Installer image
FROM mcr.microsoft.com/windows/servercore:1809 AS installer

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Retrieve .NET Core SDK
ENV DOTNET_SDK_VERSION 3.0.100-preview5-011568

RUN Invoke-WebRequest -OutFile dotnet.zip https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$Env:DOTNET_SDK_VERSION/dotnet-sdk-$Env:DOTNET_SDK_VERSION-win-x64.zip; `
    $dotnet_sha512 = '6171b234ff20849a970f750e39f3c5a968f702eacaac5d04d1ac8e2c01c779f3a4993eca42245c04e02ed063e8d59989db47796f334275f5611c40703278cbf5'; `
    if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    Expand-Archive dotnet.zip -DestinationPath dotnet; `
    Remove-Item -Force dotnet.zip

# Install PowerShell global tool
ENV POWERSHELL_VERSION 6.2.0

RUN Invoke-WebRequest -OutFile PowerShell.Windows.x64.$ENV:POWERSHELL_VERSION.nupkg https://pwshtool.blob.core.windows.net/tool/$ENV:POWERSHELL_VERSION/PowerShell.Windows.x64.$ENV:POWERSHELL_VERSION.nupkg; `
    $powershell_sha512 = 'bdd76bc99548c36506da4312ebf3acb99a8d778c644a8278f7f80533dbd89bd7858b57b487c23ebd85778841da12a557891b532960c1018e1621a8e9d12a3f8a'; `
    if ((Get-FileHash PowerShell.Windows.x64.$ENV:POWERSHELL_VERSION.nupkg -Algorithm sha512).Hash -ne $powershell_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    \dotnet\dotnet tool install --add-source . --tool-path \powershell --version $ENV:POWERSHELL_VERSION PowerShell.Windows.x64; `
    Remove-Item -Force PowerShell.Windows.x64.$ENV:POWERSHELL_VERSION.nupkg; `
    Remove-Item -Path \powershell\.store\powershell.windows.x64\$ENV:POWERSHELL_VERSION\powershell.windows.x64\$ENV:POWERSHELL_VERSION\powershell.windows.x64.$ENV:POWERSHELL_VERSION.nupkg -Force

# SDK image
FROM mcr.microsoft.com/windows/nanoserver:1809

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

# Trigger first run experience by running arbitrary cmd
RUN dotnet help
