# escape=`

FROM mcr.microsoft.com/windows/nanoserver:1809_arm

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 2.2.102

RUN curl -SL --output dotnet.zip https://dotnetcli.blob.core.windows.net/dotnet/Sdk/%DOTNET_SDK_VERSION%/dotnet-sdk-%DOTNET_SDK_VERSION%-win-arm.zip `
    && mkdir -p "%ProgramFiles%\dotnet" `
    && tar -zxf dotnet.zip -C "%ProgramFiles%\dotnet" `
    && del dotnet.zip `
    # Add NuGet cache (ARM SDK doesn't include it)
    && curl -SL --output "%ProgramFiles%\dotnet\sdk\%DOTNET_SDK_VERSION%\nuGetPackagesArchive.lzma" https://dotnetcli.blob.core.windows.net/dotnet/Sdk/%DOTNET_SDK_VERSION%/nuGetPackagesArchive.lzma

# In order to set system PATH, ContainerAdministrator must be used
USER ContainerAdministrator 
RUN setx /M PATH "%PATH%;%ProgramFiles%\dotnet"
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
