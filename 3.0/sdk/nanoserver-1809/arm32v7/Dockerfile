# escape=`

FROM mcr.microsoft.com/windows/nanoserver:1809-arm32v7

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 3.0.102

RUN curl -SL --output dotnet.zip https://dotnetcli.azureedge.net/dotnet/Sdk/%DOTNET_SDK_VERSION%/dotnet-sdk-%DOTNET_SDK_VERSION%-win-arm.zip `
    && mkdir "%ProgramFiles%\dotnet" `
    && tar -zxf dotnet.zip -C "%ProgramFiles%\dotnet" `
    && del dotnet.zip

# Install PowerShell global tool
ENV POWERSHELL_VERSION=7.0.0-preview.4 `
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetCoreSDK-NanoServer-1809-arm32

RUN curl -SL --output PowerShell.Windows.arm32.%POWERSHELL_VERSION%.nupkg https://pwshtool.blob.core.windows.net/tool/%POWERSHELL_VERSION%/PowerShell.Windows.arm32.%POWERSHELL_VERSION%.nupkg `
    && mkdir "%ProgramFiles%\powershell" `
    && "%ProgramFiles%\dotnet\dotnet" tool install --add-source . --tool-path "%ProgramFiles%\powershell" --version %POWERSHELL_VERSION% PowerShell.Windows.arm32 `
    && del PowerShell.Windows.arm32.%POWERSHELL_VERSION%.nupkg `
    && del "%ProgramFiles%\powershell\.store\powershell.windows.arm32.\%POWERSHELL_VERSION%\powershell.windows.arm32\%POWERSHELL_VERSION%\powershell.windows.arm32.%POWERSHELL_VERSION%.nupkg"

# In order to set system PATH, ContainerAdministrator must be used
USER ContainerAdministrator
RUN setx /M PATH "%PATH%;%ProgramFiles%\dotnet;%ProgramFiles%\powershell"
USER ContainerUser

# Enable detection of running in a container
ENV DOTNET_RUNNING_IN_CONTAINER=true `
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true `
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip

# Trigger first run experience by running arbitrary cmd to populate local package cache
RUN dotnet help
