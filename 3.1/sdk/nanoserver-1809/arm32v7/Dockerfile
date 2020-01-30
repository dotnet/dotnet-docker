# escape=`

FROM mcr.microsoft.com/windows/nanoserver:1809-arm32v7

ENV `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true `
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true `
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip `
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetCoreSDK-NanoServer-1809-arm32

# Install .NET Core SDK
RUN set "dotnet_sdk_version=3.1.101" `
    && call curl -SL --output dotnet.zip https://dotnetcli.azureedge.net/dotnet/Sdk/%dotnet_sdk_version%/dotnet-sdk-%dotnet_sdk_version%-win-arm.zip `
    && mkdir "%ProgramFiles%\dotnet" `
    && tar -zxf dotnet.zip -C "%ProgramFiles%\dotnet" `
    && del dotnet.zip

# Install PowerShell global tool
RUN set "powershell_version=7.0.0-rc.1" `
    && call curl -SL --output PowerShell.Windows.arm32.%powershell_version%.nupkg https://pwshtool.blob.core.windows.net/tool/%powershell_version%/PowerShell.Windows.arm32.%powershell_version%.nupkg `
    && mkdir "%ProgramFiles%\powershell" `
    && call "%ProgramFiles%\dotnet\dotnet" tool install --add-source . --tool-path "%ProgramFiles%\powershell" --version %powershell_version% PowerShell.Windows.arm32 `
    && call rmdir "%TEMP%\NuGetScratch" /S /Q `
    && call del PowerShell.Windows.arm32.%powershell_version%.nupkg `
    && call del "%ProgramFiles%\powershell\.store\powershell.windows.arm32\%powershell_version%\powershell.windows.arm32\%powershell_version%\powershell.windows.arm32.%powershell_version%.nupkg"

# In order to set system PATH, ContainerAdministrator must be used
USER ContainerAdministrator
RUN setx /M PATH "%PATH%;%ProgramFiles%\dotnet;%ProgramFiles%\powershell"
USER ContainerUser
