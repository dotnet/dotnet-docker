FROM windowsservercore

# Install .NET Core
ENV DOTNET_VERSION 1.0.0-rc3-004459-00
ENV DOTNET_DOWNLOAD_URL https://dotnetcli.blob.core.windows.net/dotnet/preview/Binaries/$DOTNET_VERSION/dotnet-win-x86.$DOTNET_VERSION.zip

RUN powershell -NoProfile -Command \
        $ErrorActionPreference = 'Stop'; \
        Invoke-WebRequest %DOTNET_DOWNLOAD_URL% -OutFile dotnet.zip; \
        Expand-Archive dotnet.zip -DestinationPath '%ProgramFiles(x86)%\dotnet'; \
        Remove-Item -Force dotnet.zip

RUN setx /M PATH "%PATH%;%ProgramFiles(x86)%\dotnet"
