FROM nanoserver

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 1.0.0-preview2-003118
ENV DOTNET_SDK_DOWNLOAD_URL https://dotnetcli.blob.core.windows.net/dotnet/preview/Binaries/$DOTNET_SDK_VERSION/dotnet-dev-win-x64.$DOTNET_SDK_VERSION.zip

RUN powershell -NoProfile -Command \
        $ErrorActionPreference = 'Stop'; \
        $httpClient = New-Object System.Net.Http.HttpClient; \
        $stream = $httpClient.GetAsync('%DOTNET_SDK_DOWNLOAD_URL%').Result.Content.ReadAsStreamAsync().Result; \
        $file = [System.IO.File]::Create('dotnet.zip'); \
        $stream.CopyTo($file); \
        $file.Close(); \
        [System.IO.Compression.ZipFile]::ExtractToDirectory('dotnet.zip', '%ProgramFiles%\dotnet'); \
        Remove-Item -Force dotnet.zip

RUN setx /M PATH "%PATH%;%ProgramFiles%\dotnet"

# Copy forwarders assemblies to workaround https://github.com/dotnet/cli/issues/2967
RUN powershell -NoProfile -Command \
        $ErrorActionPreference = 'Stop'; \
        Copy-Item -Path '%windir%\System32\forwarders\*' -Destination '%ProgramFiles%\dotnet\'; \
        Copy-Item -Path '%windir%\System32\forwarders\*' -Destination '%ProgramFiles%\dotnet\shared\Microsoft.NETCore.App\1.0.0\'
