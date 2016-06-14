FROM windowsservercore

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 1.0.0-preview2-003041
ENV DOTNET_SDK_DOWNLOAD_URL https://dotnetcli.blob.core.windows.net/dotnet/preview/Binaries/$DOTNET_SDK_VERSION/dotnet-dev-win-x86.$DOTNET_SDK_VERSION.zip

RUN powershell -NoProfile -Command \
        $ErrorActionPreference = 'Stop'; \
        Invoke-WebRequest %DOTNET_SDK_DOWNLOAD_URL% -OutFile dotnet.zip; \
        Expand-Archive dotnet.zip -DestinationPath '%ProgramFiles(x86)%\dotnet'; \
        Remove-Item -Force dotnet.zip

RUN setx /M PATH "%PATH%;%ProgramFiles(x86)%\dotnet"
