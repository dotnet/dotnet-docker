FROM nanoserver

# Install .NET Core
ENV DOTNET_VERSION 1.0.0-rc3-004408
ENV DOTNET_DOWNLOAD_URL https://dotnetcli.blob.core.windows.net/dotnet/preview/Binaries/$DOTNET_VERSION/dotnet-win-x86.$DOTNET_VERSION.zip

RUN powershell -NoProfile -Command \
        $ErrorActionPreference = 'Stop'; \
        $httpClient = New-Object System.Net.Http.HttpClient; \
        $stream = $httpClient.GetAsync('%DOTNET_DOWNLOAD_URL%').Result.Content.ReadAsStreamAsync().Result; \
        $file = [System.IO.File]::Create('dotnet.zip'); \
        $stream.CopyTo($file); \
        $file.Close(); \
        [System.IO.Compression.ZipFile]::ExtractToDirectory('dotnet.zip', '%ProgramFiles(x86)%\dotnet'); \
        Remove-Item -Force 'dotnet.zip'

RUN setx /M PATH "%PATH%;%ProgramFiles(x86)%\dotnet"

# Copy forwarders assemblies to workaround https://github.com/dotnet/cli/issues/2967
ADD ["*.dll", "/Program Files (x86)/dotnet/"]
