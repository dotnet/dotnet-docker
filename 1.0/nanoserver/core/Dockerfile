FROM microsoft/nanoserver:10.0.14393.206

# Install .NET Core
ENV DOTNET_VERSION 1.0.1
ENV DOTNET_DOWNLOAD_URL https://dotnetcli.blob.core.windows.net/dotnet/preview/Binaries/$DOTNET_VERSION/dotnet-win-x64.$DOTNET_VERSION.zip

RUN powershell -NoProfile -Command \
        $ErrorActionPreference = 'Stop'; \
        $httpClient = New-Object System.Net.Http.HttpClient; \
        $stream = $httpClient.GetAsync('%DOTNET_DOWNLOAD_URL%').Result.Content.ReadAsStreamAsync().Result; \
        $file = [System.IO.File]::Create('dotnet.zip'); \
        $stream.CopyTo($file); \
        $file.Close(); \
        [System.IO.Compression.ZipFile]::ExtractToDirectory('dotnet.zip', '%ProgramFiles%\dotnet'); \
        Remove-Item -Force dotnet.zip

RUN setx /M PATH "%PATH%;%ProgramFiles%\dotnet"
