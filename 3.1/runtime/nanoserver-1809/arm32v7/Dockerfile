# escape=`

FROM mcr.microsoft.com/windows/nanoserver:1809-arm32v7

ENV `
    # Configure web servers to bind to port 80 when present
    ASPNETCORE_URLS=http://+:80 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true

# In order to set system PATH, ContainerAdministrator must be used
USER ContainerAdministrator
RUN setx /M PATH "%PATH%;%ProgramFiles%\dotnet"
USER ContainerUser

# Install .NET Core
RUN set "dotnet_version=3.1.1" `
    && call curl -SL --output dotnet.zip https://dotnetcli.azureedge.net/dotnet/Runtime/%dotnet_version%/dotnet-runtime-%dotnet_version%-win-arm.zip `
    && mkdir "%ProgramFiles%\dotnet" `
    && tar -zxf dotnet.zip -C "%ProgramFiles%\dotnet" `
    && del dotnet.zip
