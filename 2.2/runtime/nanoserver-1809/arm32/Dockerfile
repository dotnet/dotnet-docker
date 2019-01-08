# escape=`

FROM mcr.microsoft.com/windows/nanoserver:1809_arm

# Install .NET Core
ENV DOTNET_VERSION 2.2.1

RUN curl -SL --output dotnet.zip https://dotnetcli.blob.core.windows.net/dotnet/Runtime/%DOTNET_VERSION%/dotnet-runtime-%DOTNET_VERSION%-win-arm.zip `
    && mkdir -p "%ProgramFiles%\dotnet" `
    && tar -zxf dotnet.zip -C "%ProgramFiles%\dotnet" `
    && del dotnet.zip

# In order to set system PATH, ContainerAdministrator must be used
USER ContainerAdministrator 
RUN setx /M PATH "%PATH%;%ProgramFiles%\dotnet"
USER ContainerUser

# Configure web servers to bind to port 80 when present
ENV ASPNETCORE_URLS=http://+:80 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true
