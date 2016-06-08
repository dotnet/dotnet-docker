FROM nanoserver

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 1.0.0-preview2-002990
ENV DOTNET_SDK_DOWNLOAD_URL https://dotnetcli.blob.core.windows.net/dotnet/preview/Binaries/$DOTNET_SDK_VERSION/dotnet-dev-win-x86.$DOTNET_SDK_VERSION.zip

RUN powershell -NoProfile -Command \
        $ErrorActionPreference = 'Stop'; \
        $httpClient = New-Object System.Net.Http.HttpClient; \
        $stream = $httpClient.GetAsync('%DOTNET_SDK_DOWNLOAD_URL%').Result.Content.ReadAsStreamAsync().Result; \
        $file = [System.IO.File]::Create('dotnet.zip'); \
        $stream.CopyTo($file); \
        $file.Close(); \
        [System.IO.Compression.ZipFile]::ExtractToDirectory('dotnet.zip', '%ProgramFiles(x86)%\dotnet'); \
        Remove-Item -Force 'dotnet.zip'

RUN setx /M PATH "%PATH%;%ProgramFiles(x86)%\dotnet"

# Copy forwarders assemblies to workaround https://github.com/dotnet/cli/issues/2967
ADD ["*.dll", "/Program Files (x86)/dotnet/"]
